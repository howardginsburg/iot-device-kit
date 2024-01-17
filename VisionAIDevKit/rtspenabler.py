import sys
import requests
import time
import json
import subprocess

print(f"Starting rtspenabler.py")


# Define the URL and the JSON payload
base_url = "http://127.0.0.1:1080" #"http://192.168.175.139:1080"
print(f"base_url: {base_url}")

session_cookie = None

def login():
    global session_cookie
    url = base_url + "/login"
    payload = {"username": "admin", "userpwd": "admin"}
    # Send the POST request
    response = requests.post(url, json=payload)
    print(f"login response: {response.text}")
    if 'false' in response.text:
        return False

    # Read the 'session' cookie
    session_cookie = response.cookies.get('session')
    print(f"login successful.  session_cookie: {session_cookie}")
    return True

def logoff():
    url = base_url + "/logout"
    response = requests.post(url, cookies={'session': session_cookie})
    if 'false' in response.text:
        print(f"Unable to logoff")
        return False
    
    print(f"logoff response: {response.text}")
    return True

def enablePreview():
    url = base_url + "/preview"
    response = requests.get(url, cookies={'session': session_cookie})

    #if the response contains the world 'false' then the camera is not setup for streaming.
    if 'false' in response.text:
        #Enabling streaming.
        payload = json.dumps({"switchStatus": True})
        response = requests.post(url, data=payload, cookies={'session': session_cookie})
        print(f"Enable preview response: {response.text}")
        response = requests.get(url, cookies={'session': session_cookie})
        if 'false' in response.text:
            print(f"Unable to enable preview")
            return False
    
    return True

def checkCameraStatus():
    url = base_url + "/captureimage"
    response = requests.post(url, cookies={'session': session_cookie})
    if 'Data' in response.text:
        print(f"capture successful.  response: {response}")
    else:
        print(f"Unable to capture image")
        return False
    return True

# def rebootDevice():
#     cmd = "systemctl reboot"
#     print("Sending command: %s" % cmd)
#     returnedvalue = subprocess.call(cmd, shell=True)
#     print("Returned value is: %s" % str(returnedvalue))

try:
    # Try to login.  If you fail after two attempts, reboot the device.
    if login() == False:
        print(f"Unable to login.  Trying to logout and login again")
        logoff()
        if login() == False:
            print(f"Unable to login.  Reboot the device")
            #rebootDevice()
            sys.exit(1)

    while True:
        # Try to enable preview.  If you fail after two attempts, reboot the device.
        if enablePreview() == False:
            print(f"Unable to enable preview.  Trying to logout and login again")
            logoff()
            if login() == False:
                print(f"Unable to login.  Reboot the device")
                #rebootDevice()
                sys.exit(1)
            else:
                if enablePreview() == False:
                    print(f"Unable to enable preview.  Reboot the device")
                    #rebootDevice()
                    sys.exit(1)

        if checkCameraStatus() == False:
            print(f"Unable to capture image.  Trying to logout and login again")
            logoff()
            if login() == False:
                print(f"Unable to login.  Reboot the device")
                #rebootDevice()
                sys.exit(1)

        time.sleep(300)
except Exception as e:
    print(f"An error occurred: {e}")
    #rebootDevice()
    sys.exit(1)
    