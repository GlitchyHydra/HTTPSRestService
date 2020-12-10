
### Авторизация
#### Request:

##### POST `/login/`

```json
{
    "login": str,
    "password": str
}
```

#### Response:
OK: HTTP 200
```{
"token" : str
}
```

ERROR: HTTP 400

## Фрилансер
### Просмотр открытых предложений
#### Request:
```{
"token" : str
}
```

##### GET `/orders/opened`

#### Response:
OK: HTTP 200

```json
[{
    "id": int,
    "name": str,
    "desc": str
}, ...]
```

ERROR: HTTP 400

### Просмотр своих заявок
#### Request:
```{
"token" : str
}
```

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
    "status": IN_WORK/DONE
}, ...]
```

ERROR: HTTP 400

### Добавление заявки на выполнения предложения
#### Request:
```{
"token" : str
}
```

##### POST `/orders/<id:int>/`

#### Response:
OK: HTTP 200

ERROR: HTTP 400

### Фиксация выполнения работы
#### Request:
```{
"token" : str
}
```

##### PUT `/applications/<id:int>/`

#### Response:
OK: HTTP 200

ERROR: HTTP 400

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

ERROR: HTTP 400

### Просмотр всех своих предложений
#### Request:
```{
"token" : str
}
```

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

ERROR: HTTP 400

### Изменения статуса предложения
#### Request:
```{
"token" : str
}
```

##### PUT `/orders/<id:int>/`

```json
{
    "status": IN_WORK/CLOSE
}
```
IN_WORK/CLOSE - Enum

#### Response:
OK: HTTP 200

ERROR: HTTP 400

### Утверждение заявки на предложение
#### Request:
```{
"token" : str
}
```

##### POST `/orders/<order_id:int>/applications/<application_id:int>/`

#### Response:
OK: HTTP 200

ERROR: HTTP 400

### Подтверждение выполненной работы
#### Request:
```{
"token" : str
}
```

##### PUT `/orders/<order_id>/applications/<application_id>/`

#### Response:
OK: HTTP 200

ERROR: HTTP 400

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

Для всех успешных(OK) реквестов (если выше не задан body):
```json
{
    "response": "ok"
}
```
