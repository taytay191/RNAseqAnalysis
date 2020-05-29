import csv
import concurrent.futures
import pandas
from pandas import DataFrame
import numpy as np
from numpy import array
import sys
from sklearn import linear_model
import threading

def getandwrite(path, x, batchsize):
    df = pandas.read_csv(path, header = 0, skiprows = range(1,x))
    tname = df["Changed ID"]
    inputs = df.drop(["Changed ID"], axis = 1)
    predarray = pred(inputs)
    write(tname, predarray, batchsize)

def getapreds(batchsize,path,x, multi):
    df = pandas.read_csv(path, header=0, skiprows = range(1,(x*batchsize)+1), nrows=batchsize)
    tname = df["Changed ID"]
    inputs =df.drop(["Changed ID"], axis=1)
    processes = []
    quant = batchsize//multi
#    print(quant)
    with concurrent.futures.ThreadPoolExecutor() as executor:
        # p = executor.submit(pred,inputs)
        # processes.append(p)
        for y in range(0, multi):
            temp = inputs.iloc[y*quant:(y+1)*quant,:]
            p = executor.submit(pred,temp)
            processes.append(p)
        for z in range(0,multi):
            tempname = tname.iloc[z*quant:(z+1)*quant]
            temp = processes[z].result()
            # print(temp)
            # print(tempname)
            write(tempname, temp, quant)

def pred (inputs):
    pred = reg.predict(inputs)
    return pred

def write(tname, pred, batchsize):
    with open(str(sys.argv[3]), mode='a', newline='') as data_file:
        data_writer = csv.writer(data_file, delimiter=",")
        #print(batchsize)
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
#print(totrows)
batchsizemod = 4000
multi = 50
fullbatchnum = totrows//batchsizemod

# for x in range(0, fullbatchnum):
#     getapreds(batchsizemod,str(sys.argv[2]), x, multi)
getandwrite(str(sys.argv[2]), batchsizemod*fullbatchnum, totrows % batchsizemod)

