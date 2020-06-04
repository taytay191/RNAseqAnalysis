# RNAseqAnalysis

This program is designed to manage and interpret data from The Cancer Genome Atlas as hosted by the GDC at this site: 
https://portal.gdc.cancer.gov/

Specifically, this program is designed to establish correlative relationships between gene expression data, stored as httq.seq files
under the transcriptome profiling tab of each individual case. For efficiency in upload, the data has not been added to this file.
Should you wish to access the data message my account. As of June 2020, the model has primarily been trained on Melanoma data, although
it could be substituted for any other cancer type with relative ease. This model currently has 4 distinct parts which are outlined below.

1) Data Cleaning:
The program first retrieves the patient data from individual file, generates a complete list of Gene IDs and writes the individual patient
data to a single master file.

2) Data Modulation:
The program then creates a "reference" row by finding the mean of gene expression of all the data in the master file. The program then
itterates through the complete list of genes and multiplies the genes by the interval value. This fold change data is what will be used
later in the program to ascertian the change in survival when the expression of a given gene is changed.

3) The Model:
This is where a majority of the development on this program is currently taking place. There exists infrastructure for a linear regression,
saved as "linearregression1.py", but the R2 value of the program is so low that it is more or less meaningless. The linearregression2.py
function generates a Keras model and saves it as a .h5 file (stored outside of the repository) which can later be uploaded and used, but
it similarly has issues with meeting the cutoffs for signficance. As of June 2020, the Keras models are under development and demonstrating
extensive potential, but there is still significant work ahead before it reaches an acceptable threshold.

4) Data Analysis:
After the model is built, I intent on converting it to an ONNX model and running it within the C# framework used for the rest of the 
program. I believe doing this will increase the efficiency of the program as well as decreasing the CPU and Memory taxation on the host
computer. This step will have the ability for users to add GOIDs, or groups of genes it wants the program to look at specifically 
allowing users to query the list more efficiently rather than simply searching the 60,000+ gene list in hopes of finding change in
survival.
