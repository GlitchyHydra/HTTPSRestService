
### Авторизация
#### Request:

##### POST `/login`

`"role": "freelacer" or "customer" [не обязательное поле, если не прислано, то ищется в обоих таблицах]`

```json
{
    "login": str,
    "password": str
}
```

#### Response:
OK: HTTP 200
```json
{
    "role": str,
    "token" : str
}
```

ERROR: HTTP 401

## Фрилансер
### Просмотр открытых предложений
#### Request:
##### GET `/orders/`

```json
{
    "token" : str
}
```

#### Response:
OK: HTTP 200

```json
[{
    "id": int,
    "name": str,
    "desc": str
}, ...]
```

ERROR: HTTP 400, 401

### Просмотр своих заявок
#### Request:
##### GET `/applications/`
```json
{
    "token" : str
}
```

#### Response:
OK: HTTP 200

```json
[{
    "id": int,
    "order": {
        "id": int,
        "name": str,
        "desc": str
    },
    "status": "opened" / "approved" / "closed" / "done"
}, ...]
```

ERROR: HTTP 400, 401

### Добавление заявки на выполнения предложения
#### Request:
##### POST `/orders/<order_id:int>/`

```json
{
    "token" : str
}
```

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Фиксация выполнения работы
#### Request:
##### PUT `/applications/<application_id:int>/`
```json
{
    "token" : str
}
```

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

## Заказчик
### Добавление предложения
#### Request:

##### POST `/orders/`

```json
{
    "token" : str,
    "name": str,
    "desc": str
}
```

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Просмотр всех своих предложений
#### Request:
##### GET `/orders/`
```json
{
    "token" : str,
    "status": "opened" / "in work" / "closed" / "all"
}
```
`status == "opened" by default`

#### Response:
OK: HTTP 200

```json
[{
    "id": int,
    "name": str,
    "desc": str,
    "applications": [{
        "id": int,
        "status": "status": "opened" / "approved" / "closed" / "done"
    }, ... ]
}, ...]
```

ERROR: HTTP 400, 401

### Изменения статуса предложения
#### Request:

##### PUT `/orders/<id:int>/`
```json
{
    "token" : str,
    "status": "in work" / "closed"
}
```

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Утверждение заявки на предложение
#### Request:
##### POST `/applications/<application_id:int>/`
```json
{
    "token" : str
}
```


#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Подтверждение выполненной работы
#### Request:
##### PUT `/applications/<application_id>/`

```json
{
    "token" : str
}
```

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

# Server
Для всех ошибочных(ERROR) реквестов:
```json
{
    "error": {
        "text": str, - текст ошибки
        "code": int - код ошибки
    }
}
```
