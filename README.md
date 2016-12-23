With increase in the usage of internet, every individual who use a device connected to internet are
likely vulnerable to cyber-attacks. Every individual will be a potential target to an array of cyber
threats, such as hackers, keyloggers, and Trojans that attack through unpatched security holes. This
most people who shop and bank online, are vulnerable to identity theft and other malicious attacks.
Thus maintaining a cyber hygiene is must, which are set of steps that a computer user can take to
improve their cybersecurity and better protect themselves online. The steps include but not limited to
Configure, Control, patch and repeat. These steps can be implemented by configuring and a enhancing
some features in firewalls which are already configured to minimal settings in most of the systems.
But still enhancing the firewall settings can be complex task for a non-technical user, who has little or
no knowledge about computer security.
This project proposes a tool which can help a non-technical user to implement cyber hygiene on his
system with ease. It provides an Interface for Linux and Windows users to manage the incoming and
outgoing traffic and also select a state in which his machine need to run while performing operation
and also learn about system security and cyber hygiene.
This project makes use set of rules called as Industries Best Practices, which can be used to maintain
cyber hygiene. These rules are subjected to updates, which make it partially resilient to network based
attacks. With future development in this area can lead to complete solution for tool that can achieve
cyber hygiene.

I am going to use C# windows application in order to build a GUI for windows firewall which does all
these options internally and reduces the technical complexity of the windows firewall.
In order to make the C# code to communicate with the windows firewall we need to have a user account
policy and have system administration privileges to the program. And also need to have access to
system dlls as references in order to add or edit new rules. Following are the dlls which are needed to be
used.
C://windows/system32/hnetcfg.dll
C://windows/system32/FirewallAPI.dll
The application makes use of Windows Firewall with Advanced Security API’s to build a GUI. The
API which are used are
 NATUPNPLib
 NETCONLib
 NetFwTypeLib
Form these API’s we can use the advanced Interface object named INetFwPolicy2 to access the
required actions policies and also the type of rules. This firewall API does not work for windows XP
and minimum support is windows vista.
