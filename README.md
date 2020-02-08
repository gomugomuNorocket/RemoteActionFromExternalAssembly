# RemoteActionFromExternalAssembly

This project represent automated actions between a server and a client applications.
Normally this might be more meaningful if the server and the client reside on different computers. For the sake of simplicity, both of them running on the same machine.

The whole solution governed by a command-and-control philosophy. The server knows all. The client just does the server’s bidding.

From the configuration we can defined the dll of actions that you want to send to client, this dll is independent and you can change it as you wish. The client receives the assembly of actions from the server and creates a instance at runtime. The server may require some action to be performed.

For more details see 'ReadMe.pdf' and 'demo.png'

Test scenario:
build solution, run multiple project and type 'SelectWindow Chrome' on the server to invoke the action on the client.


