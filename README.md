<h2 align="center">Текстовый редактор для процессора</h2>

<h3>Название: Разработка лексического анализатора (сканера)</h3>
<p><h3>Цель работы:</h3> Изучить назначение и принципы работы лексического анализатора в структуре компилятора. Спроектировать алгоритм (диаграмму состояний) и выполнить программную реализацию сканера для выделения лексем из входного текста. Интегрировать разработанный модуль в ранее созданный графический интерфейс языкового процессора.</p>

Сведения об авторе: сутедент группы АП-327 Плахин Даннил

<p>Вариант задания: 86. Лямбда-выражение на языке C#. <br>Func<int, int, int, int> calc = (a, b, c) => a + (b * c);</p>
Примеры правильных входных строк
<br>Func<int, int, int> calc = (a, b) => a + b;
<br>Func<int, int, int, int, int> calc = (a, b, c, d) => a + (b * c) * d;
<h4> Список допустимых лексем</h4>
<ul>
<li>func</li>
<li>int</li>
<li>Идентификатор</li>
<li>(</li>
<li>)</li>
<li><</li>
<li>></li>
<li>+</li>
<li>*</li>
<li>=</li>
<li>_(пробел)</li>
<li>;</li>
<li>,</li>
<li>=></li>
</ul>
<p><h4>Дополнительное задание</h4>Были установлены FLEX&BISON и разработана грамматика для них. Результат прдеставлен на рисунке 1. </p>
<img align="center" alt="image" src="https://github.com/user-attachments/files/25913044/010.bmp" />
<p align="center">Рисунок 1 - Успешно просканирвоанная строка</p>

<h2 align="center">Диаграмма состяний</h2>
<img align="center" alt="image" src="https://github.com/user-attachments/assets/e1cd5a68-777e-420a-a292-5de4c9d0d200" />
<p align="center">Рисунок 2 - Диаграмма состяний</p>

<h2 align="center">Тестовые примеры</h2>
Представлен пример успешной обработки одной строки. (Рисунок 3)
<img align="center" alt="image" src="https://github.com/user-attachments/assets/f725b607-9a3a-46a7-8669-d3e47f9c1b89" />
<p align="center">Рисунок 3 - Пример успешной обработки</p>
Пример с недопустимыми символами в строке. (Рисунок 4)
<img align="center" alt="image" src="https://github.com/user-attachments/files/25912483/13.bmp" />
<p align="center">Рисунок 4 - Пример с недопустимыми символами обработки</p>
Пример с многострочием. (Рисунок 5)
<img align="center" alt="image" src="https://github.com/user-attachments/files/25912729/12.bmp")"" />
<p align="center">Рисунок 5 - Пример с многострочием</p>
