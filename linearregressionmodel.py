import csv
import pandas
from pandas import DataFrame
import numpy as np
from numpy import array
import sys
from sklearn import linear_model

def getandwrite(batchsize,path,x):
    df = pandas.read_csv(path, header=0, skiprows = range(1,(x*batchsize)+1), nrows=batchsize)
    inputs =df.drop(["Changed ID"], axis=1)
    pred = reg.predict(inputs)
    tname = df["Changed ID"]
    with open(str(sys.argv[3]), mode='a', newline='') as data_file:
        data_writer = csv.writer(data_file, delimiter=",")
        for s in range(0,batchsize):
            data_writer.writerow([tname.iloc[s], pred[s]])

data = pandas.read_csv(str(sys.argv[1]))
print("Data Collection: Success")
X = data.drop(["UUID", "Survival"], axis = 1)
Y = data.Survival
print("Data Construction: Success")
reg = linear_model.LinearRegression().fit(X, Y)
print("R score: " +str(reg.score(X,Y)))
print ("Model Training: Success")
totrows = int(sys.argv[4])
batchsize = 100
fullbatchnum = totrows//batchsize
for x in range(0,fullbatchnum):
    getandwrite(batchsize, str(sys.argv[2]),x)
getandwrite(totrows%batchsize, str(sys.argv[2]), fullbatchnum*batchsize)
