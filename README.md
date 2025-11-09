# TaxiSimulation
Задание по реализовыванию подбора водителя такси на заказ клиента.


## Результаты Benchmark
| Method     | drivers_count | grid_size | Mean              | Rank |
| ---------- | ------------- | --------- | ----------------- | ---- |
| BruteForce | 100           | 1000      | 8.518 us          | 2    |
| GridBased  | 100           | 1000      | 3,908.662 us      | 3    |
| TreeBased  | 100           | 1000      | 3.744 us          | 1    |
|            |               |           |                   |      |
| BruteForce | 100           | 10000     | 7.782 us          | 2    |
| GridBased  | 100           | 10000     | 1,888,787.033 us | 3    |
| TreeBased  | 100           | 10000     | 1.436 us          | 1    |
|            |               |           |                   |      |
| BruteForce | 10000         | 1000      | 897.652 us        | 3    |
| GridBased  | 10000         | 1000      | 34.144 us         | 2    |
| TreeBased  | 10000         | 1000      | 8.244 us          | 1    |
|            |               |           |                   |      |
| BruteForce | 10000         | 10000     | 821.251 us        | 2    |
| GridBased  | 10000         | 10000     | 2,694.487 us      | 3    |
| TreeBased  | 10000         | 10000     | 4.339 us          | 1    |

По результатам во всех тестах выиграл метод бинарного дерева `TreeBased`. Средним себя показал обычный перебор. А самый нестабильным оказался метод поиска по сетке: если водителей на карте большой процент, он показывает результат лучше обычного перебора, а на большой сетке с малым числом водителей он показывает себя очень плохо.

### Графики по результатам

##### 10000 Водителей на сетке 10000х10000
<img width="1280" height="1280" alt="image" src="https://github.com/user-attachments/assets/5b7bfe70-6198-484f-a0c5-ffa6dbc2ca80" />

##### 100 Водиталей на сетке 1000х1000
<img width="1280" height="1280" alt="image" src="https://github.com/user-attachments/assets/f70e7005-8267-4e26-9de4-c77f4eb3dd6d" />

##### 150 Водиетелй на сетке 50х50
<img width="1280" height="1280" alt="image" src="https://github.com/user-attachments/assets/e628b310-f54a-4641-9afe-554115fa27e8" />



### Результат Benchmark скриншотом консоли
<img width="1591" height="493" alt="Снимок экрана 2025-11-08 124220" src="https://github.com/user-attachments/assets/164fadce-8e6a-4b82-8465-8de33c1b5215" />


