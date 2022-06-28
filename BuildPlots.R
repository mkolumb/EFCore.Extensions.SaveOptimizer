BenchmarkDotNetVersion <- "BenchmarkDotNet v0.13.1 "
dir.create(Sys.getenv("R_LIBS_USER"), recursive = TRUE, showWarnings = FALSE)
list.of.packages <- c("ggplot2", "dplyr", "gdata", "tidyr", "grid", "gridExtra", "Rcpp", "R.devices")
new.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(new.packages)) install.packages(new.packages, lib = Sys.getenv("R_LIBS_USER"), repos = "https://cran.rstudio.com/")
library(ggplot2)
library(dplyr)
library(gdata)
library(tidyr)
library(grid)
library(gridExtra)
library(R.devices)

isEmpty <- function(val){
   is.null(val) | val == ""
}

createPrefix <- function(params){ 
   separator <- "-"
   values <- params[!isEmpty(params)]
   paste(replace(values, TRUE, paste0(separator, values)), collapse = "")
}

ends_with <- function(vars, match, ignore.case = TRUE) {
  if (ignore.case)
    match <- tolower(match)
  n <- nchar(match)

  if (ignore.case)
    vars <- tolower(vars)
  length <- nchar(vars)

  substr(vars, pmax(1, length - n + 1), length) == match
}
std.error <- function(x) sqrt(var(x)/length(x))
cummean <- function(x) cumsum(x)/(1:length(x))
BenchmarkDotNetVersionGrob <- textGrob(BenchmarkDotNetVersion, gp = gpar(fontface=3, fontsize=8), hjust=1, x=1)
nicePlot <- function(p) grid.arrange(p, bottom = BenchmarkDotNetVersionGrob)
printNice <- function(p) {} # print(nicePlot(p))
ggsaveNice <- function(fileName, p, ...) {
  cat(paste0("*** Plot: ", fileName, " ***\n"))
  # See https://stackoverflow.com/a/51655831/184842
  suppressGraphics(ggsave(fileName, plot = nicePlot(p), width = 12, height = 6, ...))
  cat("------------------------------\n")
}

args <- commandArgs(trailingOnly = TRUE)
files <- if (length(args) > 0) args else list.files()[list.files() %>% ends_with("-measurements.csv")]
for (file in files) {
  title <- gsub("-measurements.csv", "", basename(file))
  title <- gsub("\\.", " - ", title)
  measurements <- read.csv(file, sep = ";")

  result <- measurements %>% filter(Measurement_IterationStage == "Result")
  if (nrow(result[is.na(result$Job_Id),]) > 0)
    result[is.na(result$Job_Id),]$Job_Id <- ""
  if (nrow(result[is.na(result$Params),]) > 0) {
    result[is.na(result$Params),]$Params <- ""
  } else {
    result$Job_Id <- trim(paste(result$Params))
  }
  result$Job_Id <- factor(result$Job_Id, levels = unique(result$Job_Id))

  timeUnit <- "ns"
  if (min(result$Measurement_Value) > 1000) {
    result$Measurement_Value <- result$Measurement_Value / 1000
    timeUnit <- "us"
  }
  if (min(result$Measurement_Value) > 1000) {
    result$Measurement_Value <- result$Measurement_Value / 1000
    timeUnit <- "ms"
  }
  if (min(result$Measurement_Value) > 1000) {
    result$Measurement_Value <- result$Measurement_Value / 1000
    timeUnit <- "sec"
  }

  resultStats <- result %>%
    group_by_(.dots = c("Target_Method", "Job_Id")) %>%
    summarise(se = std.error(Measurement_Value), Value = mean(Measurement_Value))

  benchmarkBarplot <- ggplot(resultStats, aes(x=Target_Method, y=Value, fill=Job_Id)) +
    guides(fill=guide_legend(title="Variant")) +
    xlab("Operation") +
    ylab(paste("Time,", timeUnit)) +
    ggtitle(title, subtitle="(lower is better)") +
    geom_bar(position=position_dodge(), stat="identity")

  printNice(benchmarkBarplot)
  ggsaveNice(gsub("-measurements.csv", "-barplot.png", file), benchmarkBarplot)
}
