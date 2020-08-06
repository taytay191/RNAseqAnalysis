from sklearn import preprocessing
from pandas import read_csv
from sklearn.model_selection import train_test_split
from keras.layers import Dense
from keras.models import Sequential
from keras.optimizers import Adam
from sklearn.metrics import r2_score
from matplotlib import pyplot as plt

df = read_csv("C:\Code\RNASeqAnalysis\Data\CentData.csv", header = 0)

survival = df.Survival
genelist = df.drop(["UUID", "Survival"], axis = 1)
scaler = preprocessing.StandardScaler().fit(genelist)

X_train, X_val, Y_train, Y_val = train_test_split(genelist, survival, test_size = .2)

X_tprep = scaler.transform(X_train)
X_vprep = scaler.transform(X_val)
#print(X_vprep, X_vprep, Y_train, Y_val)

model = Sequential()
model.add(Dense(750, input_shape = (60488,), activation ='relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(750, activation = 'relu'))
model.add(Dense(1,))

model.compile(Adam(lr = .00000075), 'mean_squared_error')
history = model.fit(X_tprep,Y_train, epochs = 6500, validation_split = .25, verbose = 0)

print("Model compiled and trained")

Yval_pred = model.predict(X_vprep)
rscore = r2_score(Y_val,Yval_pred)
Yval_pred2 = model.predict(X_tprep)
rscore1 = r2_score(Y_train, Yval_pred2)
print("R2 score (Validation Data): " + str(rscore))
print("R2 score (Training Data): " + str(rscore1))

model.save(R'C:\Users\tjmcg\source\repos\RNAseqFinal\RNAseqFinal\weights.h5')

history_dict = history.history
plt.figure()
plt.yscale("log")
plt.plot(history_dict['loss'], 'bo', label = 'training loss')
plt.plot(history_dict['val_loss'], 'r', label = 'val training loss')
plt.show()

