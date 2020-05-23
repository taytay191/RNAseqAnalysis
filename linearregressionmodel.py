import pandas
from pandas import DataFrame
import numpy as np
from numpy import array
import sys
from sklearn import linear_model

data = pandas.read_csv(str(sys.args[0]))
print("Data Collection: Success")
X = data.drop(["UUID", "Survival"], axis = 1)
Y = data.Survival
print("Data Construction: Success")
reg = linear_model.LinearRegression().fit(X, Y)
print("R score: " +reg.score(X,Y))
print ("Model Training: Success")
