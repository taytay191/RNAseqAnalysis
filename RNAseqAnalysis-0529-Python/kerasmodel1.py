import pandas
import sys
from keras.models import Sequential
from keras.layers import Dense
from keras.optimizers import Adam
from keras.wrappers.scikit_learn import KerasRegressor
from matplotlib import pyplot as plt
from sklearn.model_selection import cross_val_score
from sklearn.model_selection import KFold
from skleanr.model_selection import train_test_split
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipelin

def main():
    df = pandas.read_csv(str(sys.argv[1]))
    X = df.drop(["UUID", "Survival"])
    Y = df.Survival
    X_train, Xval, Y_train, Yval = train_test_split(X, Y, testsize = .1 )
    X_train  = preprocessing.scale(X_train)
    Xval = preprocessing.scale(Xval)
    model = Sequential()
    model.add(Dense(50000, input_shape =(int(sys.argv[2])), activation='relu'))
    model.add(Dense(40000, activation = 'relu'))
    model.add(Dense(40000, activation ='relu'))
    model.add(Dense(40000, activation ='relu'))
    model.add(Dense(40000, activation ='relu'))
    model.add(Dense(40000, activation ='relu'))
    model.add(Dense(1,))
    model.compile(Adam(lr=.003), 'mean_squared_error')

    history = model.fit(X_train, Y_train, epochs = 6000, validation_split =.2, verbose =0)

    history_dict = history.history
    plt.plot(history_dict['loss'],'bo', label = 'training loss')
    plt.plot(history_dict['val_loss'], 'r', label = "training loss val")
    
if __name__ == '__main__':
    main()
