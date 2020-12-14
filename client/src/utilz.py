import requests
import json
from client.src.data import Client

BASE_URL = ''

def post(path, data={}):
    r = requests.post(BASE_URL + '/' + path, json=data)
    if not r:
        # return error
        return False

def get(path, data={}):
    r = requests.post(BASE_URL + '/' + path, json=data)
    return r

def put(path, data={}):
    r = requests.put(BASE_URL + '/' + path, json=data)
    if not r:
        # return error
        return False

def login(client):
    path = 'login'
    data = client.login_data
    r = get(path, data)
    if not r:
        pass

def needed_relogin(r, client):
    if r.get('error'):
        print(r['error']['text'])
        if r['error']['code'] == 1:
            rr = relogin(client)
            if not rr:
                print('Can not login to server')
                return False
    return True


def relogin(client):
    r = login(client)
    if not r.get('error'):
        client.token = r['token']
        return True
    else:
        print(r['error']['text'])
        return False