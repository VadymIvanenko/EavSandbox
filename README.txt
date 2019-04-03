
На бэке простая EAV на MS SQL Server. Без индексов, валидации, оптимизаций.
Атрибуты только строковые (Допускаю вариации с полями или отдельными таблицами под каждый тип данных).
Работа происходит через репозиторий, который преобразует EAV структуру в JSON и обратно, 
скрывая детали реализации хранилища. Со стороны конечного пользователя веб-сервиса имеем обычный JSON API
с динамической схемой. Все новые категории сущностей и атрибуты будут добавляются динамически.

api/{db_name}/eav/{EntityType}
где db_name - имя соединения с БД, может быть установлено в db1, db2
EntityType - имя сущности (любое, напр. Patient, Club)

Поддерживается почти любая структура: 
- Строковые свойства
- Объекты
- Массивы объектов
Не поддерживаются массивы с примитивами (числами, строками) - "arr": [1, 2, 3, "foo"]

API:

POST api/{db_name}/eav/{EntityType}
POST api/db1/eav/Patient
{
    "Name": "Ivan Ivanov",
    "Gender": "Male",
    "Operation": [
        {
            "Name": "Some operation name 222",
            "Date": "04/01/2019 00:15:24",
            "Details": "Loren Ipsum dolor set amet. Bla bla."
        },
        {
            "Name": "Some operation name",
            "Date": "04/01/2019 00:15:24",
            "Details": "Loren Ipsum dolor set amet."
        }
    ],
    "Details": {
        "Bio": "Loren Ipsum dolor set amet. Loren Ipsum dolor set amet.",
        "Rh": "2(+)"
    }
}

PUT api/{db_name}/eav/{EntityType}/{EntityId}
PUT api/db1/eav/Patient/1
{
    "Id": 1,
    "Name": "Ivan2 Ivanov2",
    "Gender": "Male2",
    "Operation": [
        {
            "Id": 2,
            "Name": "Some22 operation name 222",
            "Date": "04/01/2019 00:15:24",
            "Details": "Loren22 Ipsum dolor set amet. Bla bla."
        },
        {
            "Id": 3,
            "Name": "Some22 operation name",
            "Date": "04/01/2019 00:15:24",
            "Details": "Loren22 Ipsum dolor set amet."
        }
    ],
    "Details": {
        "Id": 4,
        "Bio": "Loren222 Ipsum dolor set amet. Loren Ipsum dolor set amet.",
        "Rh": "3(+)"
    }
}

DELETE api/{db_name}/eav/{EntityType}/{EntityId}
DELETE api/db1/eav/Patient/1

GET api/{db_name}/eav/{EntityType}/expand=... # Details.InnerDetails,Operation
GET api/db1/eav/Patient?expand=Operation,Details
Response:
[
    {
        "Id": 1,
        "Name": "Ivan2 Ivanov2",
        "Gender": "Male2",
        "Operation": [
            {
                "Id": 2,
                "Name": "Some22 operation name 222",
                "Date": "04/01/2019 00:15:24",
                "Details": "Loren22 Ipsum dolor set amet. Bla bla."
            },
            {
                "Id": 3,
                "Name": "Some22 operation name",
                "Date": "04/01/2019 00:15:24",
                "Details": "Loren22 Ipsum dolor set amet."
            }
        ],
        "Details": {
            "Id": 4,
            "Bio": "Loren222 Ipsum dolor set amet. Loren Ipsum dolor set amet.",
            "Rh": "3(+)"
        }
    },
    {
        "Id": 5,
        "Name": "Ivan Ivanov",
        "Gender": "Male",
        "Operation": [
            {
                "Id": 6,
                "Name": "Some operation name 222",
                "Date": "04/01/2019 00:15:24",
                "Details": "Loren Ipsum dolor set amet. Bla bla."
            },
            {
                "Id": 7,
                "Name": "Some operation name",
                "Date": "04/01/2019 00:15:24",
                "Details": "Loren Ipsum dolor set amet."
            }
        ],
        "Details": {
            "Id": 8,
            "Bio": "Loren Ipsum dolor set amet. Loren Ipsum dolor set amet.",
            "Rh": "2(+)"
        }
    }
]