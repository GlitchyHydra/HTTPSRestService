### Токен
`Располагается в заголовке по типу Authorization: <type=Bearer> <credentials=token>`
`При каждом запросе, помимо Авторизации`

### Авторизация
#### Request:

##### POST `/login`

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
    "status": "Open" / "Rejected" / "Accepted"
}, ...]
```

ERROR: HTTP 400, 401

### Добавление заявки на выполнения предложения
#### Request:
##### POST `/orders/<order_id:int>/`

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Фиксация выполнения работы
#### Request:
##### PUT `/works/<work_id:int>/`
```json
{
    "status": str (Done)
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
    "name": str,
    "desc": str (not required)
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
    "status": "Open" / "Processing" / "Close"
}
```
`status == "Open" by default`

#### Response:
OK: HTTP 200

```json
[{
    "id": int,
    "name": str,
    "desc": str,
    "applications": [{
        "id": int,
        "status": "status": "Open" / "Rejected" / "Accepted"
    }, ... ]
}, ...]
```

ERROR: HTTP 400, 401

### Изменения статуса предложения
#### Request:

##### PUT `/orders/<id:int>/`
```json
{
    "status": "Processing" / "Close"
}
```

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Утверждение заявки на предложение
#### Request:
##### PUT `/applications/<application_id:int>/`

#### Response:
OK: HTTP 200

ERROR: HTTP 400, 401

### Подтверждение выполненной работы
#### Request:
##### PUT `/works/<work_id>/`

```json
{
    "status": str (Close)
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
    }
}
```
