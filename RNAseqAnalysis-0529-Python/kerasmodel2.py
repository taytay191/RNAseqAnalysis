from keras.layers import Dense
from keras.models import Sequential
from keras.optimizers import Adam
from matplotlib import pyplot as plt
import pandas as pd
from pandas import DataFrame
from sklearn.metrics import r2_score
from sklearn.model_selection import train_test_split
from sklearn import preprocessing 
import sys

print("Script Loaded Successfully")
df = pd.read_csv("C:\Code\RNASeqData\generateddata\cleaned\data.csv", header = 0)
X_train, Xval, Y_train, Yval = train_test_split(df.drop(["UUID","Survival"], axis =1), df.Survival, test_size = .1)
#print(inputlen)
#print(X_train, Xval)
#X_train = preprocessing.scale(X_train)
#Xval = preprocessing.scale(Xval)
#print(X_train, Xval)
model = Sequential()
model.add(Dense(1000,input_shape = (60488,), activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(1000, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(1000, activation = 'relu'))
model.add(Dense(1,))

model.compile(Adam(lr = .000000025), 'mean_squared_error')

history = model.fit(X_train, Y_train, epochs = 1250, validation_split = 0.1, verbose = 0)

Yval_pred = model.predict(Xval)
rscore = r2_score(Yval,Yval_pred)
Yval_pred2 = model.predict(X_train)
rscore1 = r2_score(Y_train, Yval_pred2)

print("R2 score (Validation Data): " + str(rscore))
print("R2 score (Training Data): " + str(rscore1))

history_dict = history.history
plt.figure()
plt.plot(history_dict['loss'], 'bo', label = 'training loss')
plt.plot(history_dict['val_loss'], 'r', label = 'val training loss')
plt.show()

model.save('C:\Code\RNAseqAnalysis-0529-Python\Model Saves\Deep\weights.h5')

print("Finished")


