import requests
import json
from client.src.data import Client

BASE_URL = ''


def post(path, data={}, headers={}):
    result = None
    try:
        result = _post(path, data, headers)
    except RequestError as e:
        result = {'error': e.message, 'request': e.request}
    return result


def _post(path, data, headers):
    r = requests.post(BASE_URL + '/' + path, json=data, headers=headers)
    if r.status_code == 200:
        return r
    else:
        raise RequestError(path, r)


def get(path, data={}, headers={}):
    result = None
    try:
        result = _get(path, data, headers)
    except RequestError as e:
        result = {'error': e.message, 'request': e.request}
    return result


def _get(path, data, headers):
    r = requests.get(BASE_URL + '/' + path, json=data, headers=headers)
    if r.status_code == 200:
        return r
    else:
        raise RequestError(path, r)


def put(path, data={}, headers={}):
    result = None
    try:
        result = _put(path, data, headers)
    except RequestError as e:
        result = {'error': e.message, 'request': e.request}
    return result


def _put(path, data, headers):
    r = requests.put(BASE_URL + '/' + path, json=data, headers=headers)
    if r.status_code == 200:
        return r
    else:
        raise RequestError(path, r)


def login(client):
    path = 'login/'
    data = client.login_data
    return get(path, data)


def needed_relogin(r, client):
    if r.get('error'):
        print(r['error'])
        if r['request'].status_code == 401:
            if not relogin(client):
                print('Can not login to server')
                return True
        else:
            print('Something wrong with server')
            return True
    return False


def relogin(client):
    r = login(client)
    if not r.get('error'):
        client.token = r['token']
        return True
    else:
        print(r['error'])
        return False


class RequestError(Exception):
    def __init__(self, path, request):
        super().__init__(message)
        self.message = f'Error with request to {BASE_URL + path} with {request.json()["error"]}'
        self.request = request
