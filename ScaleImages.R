if (!require("BiocManager", quietly = TRUE))
{
    install.packages("BiocManager", repos = "https://cran.rstudio.com/")
    BiocManager::install("EBImage")
}

library("EBImage")
library(dplyr)
ends_with <- function(vars, match, ignore.case = TRUE) {
  if (ignore.case)
    match <- tolower(match)
  n <- nchar(match)

  if (ignore.case)
    vars <- tolower(vars)
  length <- nchar(vars)

  substr(vars, pmax(1, length - n + 1), length) == match
}

args <- commandArgs(trailingOnly = TRUE)
files <- if (length(args) > 0) args else list.files()[list.files() %>% ends_with("-barplot.png")]

for (file in files) {
  cat(paste0("*** File: ", file, " ***\n"))

    x <- readImage(file)

    dim(x)[1:2]

    y <- resize(x, 1200)

    writeImage(y, gsub("-barplot.png", "-barplot-small.png", file), quality = 80)
}