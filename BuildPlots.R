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
  suppressGraphics(ggsave(fileName, plot = nicePlot(p), width = 8, height = 4, ...))
  cat("------------------------------\n")
}

args <- commandArgs(trailingOnly = TRUE)
files <- if (length(args) > 0) args else list.files()[list.files() %>% ends_with("-report-github.csv")]
for (file in files) {
  title <- gsub("-report-github.csv", "", basename(file))
  title <- gsub("\\.", " - ", title)
  title <- gsub("CockroachMulti", "CockroachDB (9 nodes)", title)
  title <- gsub("Cockroach -", "CockroachDB (1 node) -", title)
  title <- gsub("SqlServer", "SQL Server", title)
  title <- gsub("Oracle", "Oracle Express", title)
  title <- gsub("PomeloMySql", "MySQL", title)
  title <- gsub("PomeloMariaDb", "MariaDB", title)
  title <- gsub("Postgres", "PostgreSQL", title)
  title <- gsub("Sqlite", "SQLite", title)
  title <- gsub("Firebird3", "Firebird 3", title)
  title <- gsub("Firebird4", "Firebird 4", title)
  measurements <- read.csv(file, sep = ";")

  result <- measurements
  if (nrow(result[is.na(result$Method),]) > 0)
    result[is.na(result$Method),]$Method <- ""
  if (nrow(result[is.na(result$Rows),]) > 0) {
    result[is.na(result$Rows),]$Rows <- ""
  } else {
    result$Method <- trim(paste(result$Rows))
  }
  result$Method <- factor(result$Method, levels = unique(result$Method))

  timeUnit <- "ns"
  if (min(result$Measurement) > 1000) {
    result$Measurement <- result$Measurement / 1000
    timeUnit <- "us"
  }
  if (min(result$Measurement) > 1000) {
    result$Measurement <- result$Measurement / 1000
    timeUnit <- "ms"
  }
  if (min(result$Measurement) > 1000) {
    result$Measurement <- result$Measurement / 1000
    timeUnit <- "sec"
  }

  resultStats <- result %>%
    group_by(.dots = c("Variant", "Method")) %>%
    summarise(se = std.error(Measurement), Value = mean(Measurement))

  benchmarkBarplot <- ggplot(resultStats, aes(x=Variant, y=Value, fill=Method)) +
    guides(fill=guide_legend(title="Rows")) +
    xlab("Save changes") +
    ylab(paste("Time,", timeUnit)) +
    ggtitle(title, subtitle="(lower is better)") +
    geom_bar(position=position_dodge(), stat="identity")

  printNice(benchmarkBarplot)
  ggsaveNice(gsub("-report-github.csv", "-barplot.png", file), benchmarkBarplot)
}
