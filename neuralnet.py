import pandas
import numpy as np
from numpy import array
#from sklearn import linear_model

data = pandas.read_csv("C:\DataStorage\data.csv")
#print(data)
df = pandas.read_csv("C:\DataStorage\ATR.csv")
#print(df)
command = df.values
genelist = ""
for row in command:
    genelist = genelist+ str(command[row])[2:-2] + " "
print(genelist)
#print(genelabels)
#print(genearray[30])
print("Data Construction: Success")
#reg = linear_model.LinearRegression()
#reg.fit(data[[command]],data.Survival)
#print ("Model Training: Success")
