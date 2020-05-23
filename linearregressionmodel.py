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
