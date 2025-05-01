# Noldus_LSL

This is a really simple command line executable for Windows that allows you to send a LSL trigger to NIRx at the start and stop of a recording in Noldus' Observer software

How it works:
1) In the Observer, add a new device to the observation setup.  This is the same way you add calls to mediarecorder.
     Add the Noldus_LSL.exe to be called by the StartObservation trigger.  This will launch the code in the back ground and open up a LSL thread. 
 Important- the LSL thread must be created before you start recording with Aurora because it only looks for the thread right before data starts flowing.  Thus, the New Observation must be created before you start a NIRx recording
2) Add the Noldus_LSL.exe again to the second entry triggered when the recording starts.  You can use Noldus_LSL.exe #, where # is 1,2,3 etc to give a specific trigger code (or if left blank it will use "1")
3) [Optional] add Noldus_LSL.exe to the stop recording trigger to add a stop mark in the data
4) Add "Noldus_LSL.exe stop" to the close obervation to make sure the backend code exits and closes the LSL thread when you are done (not the end of the world if you forget this, but good practice)

       
