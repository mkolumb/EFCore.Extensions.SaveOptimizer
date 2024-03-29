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

BenchmarkSmallDotNetVersionGrob <- textGrob(BenchmarkDotNetVersion, gp = gpar(fontface=3, fontsize=14), hjust=1, x=1)
nicePlotSmall <- function(p) grid.arrange(p, bottom = BenchmarkSmallDotNetVersionGrob)

ggsaveNice <- function(fileName, p, ...) {
  cat(paste0("*** Plot: ", fileName, " ***\n"))
  # See https://stackoverflow.com/a/51655831/184842
  suppressGraphics(ggsave(fileName, plot = nicePlot(p), width = 8, height = 4, ...))
  cat("------------------------------\n")
}

ggsaveNiceSmall <- function(fileName, p, ...) {
  cat(paste0("*** Plot: ", fileName, " ***\n"))
  # See https://stackoverflow.com/a/51655831/184842
  suppressGraphics(ggsave(fileName, plot = nicePlotSmall(p), width = 16, height = 8, scale = 1, dpi = 72, ...))
  cat("------------------------------\n")
}

args <- commandArgs(trailingOnly = TRUE)
files <- if (length(args) > 0) args else list.files()[list.files() %>% ends_with("-measurements.csv")]
for (file in files) {
  title <- gsub("-measurements.csv", "", basename(file))
  title <- gsub("\\.", " - ", title)
  title <- gsub("CockroachMulti", "CockroachDB (9 nodes)", title)
  title <- gsub("Cockroach -", "CockroachDB (1 node) -", title)
  title <- gsub("SqlServer", "SQL Server", title)
  title <- gsub("Oracle21", "Oracle Express 21", title)
  title <- gsub("PomeloMySql", "MySQL", title)
  title <- gsub("PomeloMariaDb", "MariaDB", title)
  title <- gsub("Postgres", "PostgreSQL", title)
  title <- gsub("Sqlite", "SQLite", title)
  title <- gsub("Firebird3", "Firebird 3", title)
  title <- gsub("Firebird4", "Firebird 4", title)
  measurements <- read.csv(file, sep = ";")

  result <- measurements %>% filter(Measurement_IterationStage == "Result")
  if (nrow(result[is.na(result$Job_Id),]) > 0)
    result[is.na(result$Job_Id),]$Job_Id <- ""
  if (nrow(result[is.na(result$SavedRows),]) > 0) {
    result[is.na(result$SavedRows),]$SavedRows <- ""
  } else {
    result$Job_Id <- trim(paste(result$SavedRows))
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
    group_by_(.dots = c("SaveVariant", "Job_Id")) %>%
    summarise(se = std.error(Measurement_Value), Value = median(Measurement_Value, na.rm = TRUE))

  benchmarkBarplot <- ggplot(resultStats, aes(x=SaveVariant, y=Value, fill=Job_Id)) +
    guides(fill=guide_legend(title="Rows")) +
    xlab("Save changes") +
    ylab(paste("Time,", timeUnit)) +
    ggtitle(title, subtitle="(median, lower is better)") +
    geom_bar(position=position_dodge(), stat="identity")
    #geom_errorbar(aes(ymin=Value-1.96*se, ymax=Value+1.96*se), width=.2, position=position_dodge(.9))

  smallBenchmarkBarplot <- ggplot(resultStats, aes(x=SaveVariant, y=Value, fill=Job_Id)) +
    guides(fill=guide_legend(title="Rows")) +
    xlab("Save changes") +
    ylab(paste("Time,", timeUnit)) +
    ggtitle(title, subtitle="(median, lower is better)") +
    geom_bar(position=position_dodge(), stat="identity") + 
    theme(plot.title = element_text(size=26, margin=margin(t=10,r=0,b=0,l=0)), 
    plot.subtitle = element_text(size=20, margin=margin(t=10,r=0,b=10,l=0)), 
    axis.title.x = element_text(size=22, margin=margin(t=10,r=10,b=0,l=10)), 
    axis.title.y = element_text(size=22, margin=margin(t=10,r=10,b=10,l=10)), 
    axis.text.x = element_text(size=16, margin=margin(t=10,r=10,b=0,l=10)), 
    axis.text.y = element_text(size=16, margin=margin(t=10,r=10,b=10,l=0)), 
    legend.title = element_text(size=18, margin=margin(t=10,r=0,b=10,l=0)), 
    legend.text = element_text(size=15, margin=margin(t=5,r=0,b=5,l=0)),
    legend.key.width = unit(25, "pt"),
    legend.key.height = unit(25, "pt"))

  ggsaveNice(gsub("-measurements.csv", "-barplot.png", file), benchmarkBarplot)
  ggsaveNiceSmall(gsub("-measurements.csv", "-barplot-small.png", file), smallBenchmarkBarplot)
}
