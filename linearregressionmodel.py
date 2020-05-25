import csv
import pandas
from pandas import DataFrame
import numpy as np
from numpy import array
import sys
from sklearn import linear_model

#print(sys.argv[1])
data = pandas.read_csv(str(sys.argv[1]))
print("Data Collection: Success")
X = data.drop(["UUID", "Survival"], axis = 1)
Y = data.Survival
print("Data Construction: Success")
reg = linear_model.LinearRegression().fit(X, Y)
print("R score: " +str(reg.score(X,Y)))
print ("Model Training: Success")
#pred = reg.predict(X)
#print(pred)
#with open(str(sys.argv[2]), mode='w') as data_file:
#    data_writer = csv.writer(data_file, delimiter =',')
#    data_writer.writerow(pred)
print(sys.argv[2])
df = pandas.read_csv(str(sys.argv[2]), nrows=10)
print(df)
#inputs = df.drop(["Gene Changed"], axis =1)
#outputs = reg.predict(inputs)
#with open(str(sys.argv[3], mode='w')) as data_file:
#    data_writer = csv.writer(data_file,delimiter=',')
#    data_writer.writerow(outputs)
