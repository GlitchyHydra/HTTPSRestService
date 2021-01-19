import requests
import json

BASE_URL = 'https://glitchyhydra.ddns.net'


def post(path, data={}, headers={}, params={}):
    result = None
    try:
        result = _post(path, data, headers, params).json()
    except RequestError as e:
        result = {'error': e.message, 'responce': e.r}
    return result


def _post(path, data, headers, params):
    r = requests.post(BASE_URL + '/' + path, json=data,
                      headers=headers, params=params)
    if r.status_code == 200 and not (isinstance(r.json(), dict) and r.json().get('error')):
        return r
    else:
        raise RequestError(path, r)


def get(path, data={}, headers={}, params={}):
    result = None
    try:
        result = _get(path, data, headers, params).json()
    except RequestError as e:
        result = {'error': e.message, 'responce': e.r}
    return result


def _get(path, data, headers, params):
    r = requests.get(BASE_URL + '/' + path, json=data,
                     headers=headers, params=params)
    if r.status_code == 200 and not (isinstance(r.json(), dict) and r.json().get('error')):
        return r
    else:
        raise RequestError(path, r)


def put(path, data={}, headers={}, params={}):
    result = None
    try:
        result = _put(path, data, headers, params).json()
    except RequestError as e:
        result = {'error': e.message, 'responce': e.r}
    return result


def _put(path, data, headers, params):
    r = requests.put(BASE_URL + '/' + path, json=data,
                     headers=headers, params=params)
    if r.status_code == 200 and not (isinstance(r.json(), dict) and r.json().get('error')):
        return r
    else:
        raise RequestError(path, r)


def login(client):
    path = 'login/'
    data = client.login_data
    return post(path, data)


def needed_relogin(r, client):
    if isinstance(r, dict) and r.get('error'):
        print(r['error'])
        if r['responce'].status_code == 401:
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
    def __init__(self, path, r):
        self.message = f'Error with request to {BASE_URL + "/" + path} with {r.json()["errorText"] if r.text else r.reason}'
        super().__init__(self.message)
        self.r = r
