# AnnyConnectController
[Japanese README](README.ja.md)

## About
- This is a tool to automatically input the PIN for Cisco AnyConnect Secure Mobility Client with just one click.

- The PIN is encrypted and saved using Data Protection API (DPAPI).


## Installation
- Download zip file from https://github.com/saltpp/AnyConnectController/releases and extract it
- Place AnyConnectController.exe and AnyConnectController.exe.Config in any directory.


## Uninstallation
- Remove AnyConnectController.exe and AnyConnectController.exe.Config.
- No registry entries are used.


## Usage
- Start the tool, enter the PIN, and press the Connect button. The PIN will be saved for easier access.


## Development notes
- The PATH of Cisco AnyConnect Secure Mobility Client may vary depending on the environment. If that's the case, modify FULL_PATH_ANY_CONNECT in Constants class, then rebuild the project.

- The CLI command (vpncli.exe) is available and can automatically input the PIN for connection via the command line. However, when Cisco AnyConnect Secure Mobility Client is running, connection is not possible. So I made this tool to input the PIN to Cisco AnyConnect Secure Mobility Client.
  - Write Username + CR/LF and PIN in in.txt
    ```
    Username
    PIN
    ```
    then connect with the following command:
    ```
    vpncli.exe -s connect <server> < in.txt
    ```
    Disconnect with the following command:
    ```
    vpncli.exe disconnect
    ```
## History
- Ver.1.0.0
  - Initial release


## License
- MIT


## Others
- [Buy me a coffee](https://www.buymeacoffee.com/saltpp)