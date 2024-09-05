if(!require("clusterProfiler")){
  source("http://bioconductor.org/biocLite.R")
  biocLite("clusterProfiler")
}
library("clusterProfiler")

workFolder <- "E:\\sam_dog_seal\\rpkm\\"
rSubReadFile <- "E:\\sam_dog_seal\\rpkm\\gene_counts_withDups_unstranded.txt"
setwd(workFolder)
counts <- read.delim(rSubReadFile)

rownames(counts) <- gsub("\\..*", "", rownames(counts))

rowN <- rownames(counts)
head(rowN)
names<- bitr(geneID=rownames(counts), fromType = "REFSEQ",toType = "SYMBOL", OrgDb = "org.Cf.eg.db")
head(names)
counts$RefSeq <- rownames(counts)

merged <- merge(counts, names, by.x="RefSeq", by.y="REFSEQ", all.x=TRUE, all.y=FALSE)
)

write.table(merged, file = "counts_with_sybmold.xls", append = FALSE, quote = FALSE, sep = "\t",
            eol = "\n", na = "NA", dec = ".", row.names = TRUE,
            col.names = TRUE, qmethod = c("escape", "double"),
            fileEncoding = "")
