import csv
import concurrent.futures
import pandas
from pandas import DataFrame
import numpy as np
from numpy import array
import sys
import sklearn
from sklearn import linear_model
import threading

def main():
    def pred (inputs):
        pred = reg.predict(inputs)
        return pred
    def write(tname, pred, batchsize):
#        print("F.Python5")
        with open(str(sys.argv[3]), mode='a', newline='') as data_file:
            data_writer = csv.writer(data_file, delimiter=",")
            #print(batchsize)
            for s in range(0,batchsize):
                data_writer.writerow([tname.iloc[s], pred[s]])
    def getapreds(batchsize,path,x, multi, ob):
        df = pandas.read_csv(path, header = 0, skiprows =range(1,(x*batchsize)), nrows = batchsize)
        tname.append(df["Changed ID"])
        inputs = df.drop(["Changed ID"], axis = 1)
        processes = []
        tnamearray = []
        quant = batchsize//multi
        rem = batchsize%multi
        with concurrent.futures.ThreadPoolExecutor() as executor:
            for y in range(0, multi):
                p = executor.submit(pred,inputs[y*quant,(y+1)*quant])
                processes.append(p)
                temp = tname[y*quant,(y+1)*quant]
                tnamearray.append(temp)
            for z in range(0,multi):
                tempname = tnamearray[z]
                temp = processes[z].result()
                write(tempname, temp, quant)
            if (rem > 0):
                dfrem = pandas.read_csv(path, header = 0, skiprows= range(1, (x*ob) +(multi*quant)), nrows = rem)
                remname = dfrem["Changed ID"]
                reminputs = dfrem.drop(["Changed ID"], axis = 1)
                remout = reg.predict(reminputs)
                write(remname, remout, rem)
    data = pandas.read_csv(str(sys.argv[1]))
    valdata = pandas.read_csv(str(sys.argv[6]))
    print("Data Collection: Success")
    X = data.drop(["UUID", "Survival"], axis = 1)
    Y = data.Survival
    Xval = valdata.drop(["UUID","Survival"], axis = 1)
    Yval = valdata.Survival
    print("Data Construction: Success")
    reg = linear_model.LinearRegression().fit(X, Y)
    Ypred = reg.predict(Xval)
    rscore = sklearn.metrics.r2_score(Yval, Ypred)
    print("R Squared Value: " +str(rscore))
    print ("Model Training: Success")
    totrows = int(sys.argv[4])
#    print(totrows)
    batchsizemod = 1250
    multi = 5
    fullbatchnum = totrows//batchsizemod
    rem = totrows%batchsizemod
    maxrem = fullbatchnum*batchsizemod
    for x in range(0, 1):
        getapreds(batchsizemod,str(sys.argv[2]), x, multi, batchsizemod)
    #getapreds(rem, str(sys.argv[2]), fullbatchnum, multi, batchsizemod)


if __name__ == "__main__":
#    print("F.Python1")
    main()

