# Welcome to the Ticket Philadelphia Mini-Queue

## Disclaimer
The Software Product is provided 'as is' without any express or implied warranty of any kind, including but not limited to any warranties of merchantability, noninfringement, or fitness of a particular purpose. It is released under the GPL (General Public License)

The API used in this project is not officially supported or documented by Cisco for external use. It may be modified anytime by any patch to UCCX, Finesse, or otherwise. The schema may change at any time. This application will break if the API is changed to any extent. 

## About
This project was created to replace the mini-queue originally provided by the Cisco Agent Desktop application. In the original design, the mini-queue would provide the user with two pieces of information:

1. The number of calls currently waiting across all queues
2. The amount of time the longest-waiting currently queued call has been waiting, in the form mm:ss

Cisco Finesse does not offer support for this functionality natively, and adding a TOTALCOUNT row to a CUIC report requires a $25,000 Premium CUIC license. 

## Functionality
This program queries the Finesse realtime API located at http://$FinesseURL:9080/realtime/schema on coresident Finesse deployments with Unified CCX. 

This API is not officially supported as a method by which to obtain this data, however no method is available otherwise, so we are using what we have.

The program calls the API every 5 seconds, asynchronously, and passes control back to the application for the update. Doing this aynchonously - disposing of each of our web requests in unmanaged memory when we're done with them - results in a very low client-side resource footprint. Server-side resource cost is included in the Server Impact section below

## Authentication
The authentication is basic auth using Finesse credentials. Any user has access to the realtime API, so even an agent can use this queue. 

It will return queue data about _all_ queues at this point, although there is a TODO in place to enable the restriction of the mini-queue to only queues that one is assigned. Unfortunately, this requires the use of the alternate web services API to grab the user object - server load is a concern here, and further testing is required.

Note: Authentication _may_ not be required for this API, but we're going to keep it in anyway, for now

## Resource Utilization
Total Data throughput in KBps
\frac{[(NumCSQs * 860b) * NumUsers] * 1024}{5}
