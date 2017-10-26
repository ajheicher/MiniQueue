# Welcome to the Ticket Philadelphia Mini-Queue
This project was created to replace the mini-queue originally provided by the Cisco Agent Desktop application. In the original design, the mini-queue would provide the user with two pieces of information:

1. The number of calls currently waiting across all queues
2. The amount of time the longest-waiting currently queued call has been waiting, in the form mm:ss

Cisco Finesse does not offer support for this functionality natively, and adding a TOTALCOUNT row to a CUIC report requires a $25,000 Premium CUIC license. 

## So, how does it work?
This program queries the Finesse realtime API located at http://<FinesseURL>:9080/realtime/schema on coresident Finesse deployments with Unified CCX. 

This API is not officially supported as a method by which to obtain this data, however no method is available otherwise, so we are using what we have.

## What is the server load like?
TODO: Update this area after testing
