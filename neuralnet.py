import pandas
import numpy as np
from numpy import array
from sklearn import linear_model

data = pandas.read_csv("C:\DataStorage\data.csv")
df = pandas.read_csv("C:\DataStorage\ATR.csv")
print(df.values)
print("Data Construction: Success")

reg = linear_model.LinearRegression()
reg.fit([data[df.values]],data.Survival)
print ("Model Training: Success")
