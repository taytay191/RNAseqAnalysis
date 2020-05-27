import csv
import pandas
from pandas import DataFrame
import numpy as np
from numpy import array
import sys
from sklearn import linear_model
import multiprocessing

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
batchsizemod = 20
multi = 5
fullbatchnum = totrows%(batchsizemod*multi)
for x in range(0,fullbatchnum):
    processes = []
    for x1 in range(0, multi):
        p = multiprocessing.Process(target = getandwrite, args=[batchsizemod, str(sys.argv[2]), (multi*x)+x1])
        processes.append(p)
    processes.start()
    processes.join()
getandwrite(totrows%(batchsizemod*multi), str(sys.argv[2]), fullbatchnum*batchsizemod*multi)
