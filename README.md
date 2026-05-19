<h2 align="center">Текстовый редактор для процессора</h2>

<h3  align="center">Название: Создание внутренней формы представления программы</h3>
<p><h3 align="center">Цель работы:</h3> Изучить методы построения внутреннего представления программы (ВПП) на основе контекстно-свободной грамматики, реализовать синтаксический анализатор методом рекурсивного спуска и преобразовать арифметические выражения в тетрады и ПОЛИЗ. </p>

Сведения об авторе: cтудент группы АП-327 Плахин Даннил

<p><h3 align="center">Постановка задачи:</h3> Развить ранее созданный синтаксический анализатор (парсер) до семантического: построить абстрактное синтаксическое дерево (AST) и реализовать проверку контекстно-зависимых условий в соответствии с индивидуальным вариантом курсовой работы. Вариант задания: C# </p>
Грамматика
E → TA<br>
A → ε | + TA | - TA<br>
T → FB<br>
B → ε | * FB | / FB | % FB<br>
F → num | id | (E)<br>
id → letter {letter | digit | _}<br>
num → digit {digit}<br>
<h3 align="center">Схема сканера</h3>
<img width="525" height="692" alt="image" align="center" src="https://github.com/user-attachments/assets/aa67f0ce-1338-45fe-90db-de0feea24046" />

<br>
<h3 align="center">Схема парсера</h3>
<img width="503" height="265" alt="image" align="center" src="https://github.com/user-attachments/assets/cc07d321-47ef-4832-8c94-8a8bcf43c286" />

<h3  align="center"> Классификация грамматики по Хомскому: контекстно-свободная грамматика:</h3>
A -> α, A ∊ VN, α ∊ V*

<h3  align="center"> Примеры правильных входных строк</h3>
<br>a + b * c / 2 - q % (d + 2) - q
<br>a + b * c + d - r
<br>123 + (144 % 78 * (53 / 2))

<h3  align="center"> Лексические и сентаксические ошибки</h3>
<img width="1918" height="727" alt="image" align="center" src="https://github.com/user-attachments/assets/646055c3-ec6a-49f2-bf54-5ca280e3224e" />
<p align="center"> Рисунок 1 - Прмиер корректной строки </p>
<img width="1403" height="505" alt="image" align="center" src="https://github.com/user-attachments/assets/4b040683-b392-401a-bb61-095716f07546" />
<p align="center"> Рисунок 2 - Прмиер  строки c недоспустимыми символами </p>
<img width="1385" height="427" alt="image"  align="center" src="https://github.com/user-attachments/assets/effc5de2-1d0f-41ab-b868-3f1dc9f5acd3" />
<p align="center"> Рисунок 3 - Прмиер  строки с некоррутной структурой </p>


<h3  align="center"> Тетрады </h3>
<img width="1400" height="271" alt="image" align="center" src="https://github.com/user-attachments/assets/7188c92a-666d-4642-bcdd-53bbfac48f7c" />
<p align="center"> Рисунок 4 - Таблица тетрад </p>

<h3  align="center"> Полис </h3>
<img width="974" height="203" alt="image"  align="center" src="https://github.com/user-attachments/assets/2662ca72-bc9d-4daf-9070-f5e24fe72602" />
<p align="center"> Рисунок 5 - Полис для выражения  123 + (144 % 78 * (53 / 2))</p>






































