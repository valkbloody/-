<h2 align="center">Текстовый редактор для процессора</h2>

<h3  align="center">Название: Построение AST и проверка контекстно-зависимых условий</h3>
<p><h3 align="center">Цель работы:</h3> Изучить назначение и принципы работы семантического анализатора в структуре компилятора. Освоить методы построения абстрактного синтаксического дерева (AST) и проверки контекстно-зависимых условий (семантических правил) для заданной синтаксической конструкции. </p>

Сведения об авторе: cтудент группы АП-327 Плахин Даннил

<p><h3 align="center">Постановка задачи:</h3> Развить ранее созданный синтаксический анализатор (парсер) до семантического: построить абстрактное синтаксическое дерево (AST) и реализовать проверку контекстно-зависимых условий в соответствии с индивидуальным вариантом курсовой работы. Вариант задания: 86. Лямбда-выражение на языке C#.</p>
Примеры правильных входных строк
<br>Func&ltint, int, int&gt calc = (a, b) => a + b;
<br>Func&ltint, int, int, int, int&gt calc = (a, b, c, d) => a + (b * c) * d;
<br>Func&ltint, int, int, int&gt calc = (a, b, c) => a + (b * c);

<h3 align="center"> Контекстно-зависимые условия </h3>
<ul>
  <li>Правило 1 (уникальность идентификаторов)</li>
  <ul>
  <li>Проверка: на уникальность имен в одной зоне области</li>
  <li>При ошибке ожидаемое сообщение: Идентификатор уже был объявлен ранее: (Рисунок 1)</li>
    <img align="center" width="2500" height="200" alt="image" src="https://github.com/user-attachments/assets/f101314e-b7e3-4cac-b4eb-8f04f50f6c0b" />
    <p align="center">Рисунок 1.Ошибка уникальности имен</p>
  </ul>
  <li>Правило 4 (использование идентификаторов)</li>
  <ul>
  <li>Проверка: на то, что используемые идентификаторы были объявлены ранее</li>
  <li>При ошибке ожидаемое сообщение: Используется не объявленный идентификатор: (Рисунок 2)</li>
    <img align="center" width="2500" height="200" alt="image" src="https://github.com/user-attachments/assets/7c6850c4-b032-4999-9416-1bd5e50ae46d" />
    <p align="center">Рисунок 2.Ошибка использование неинициализированной переменной</p>
  </ul>
</ul> 

<h3 align="center"> Структура AST </h3>
<ul>
  <p>
  <li>Узел AstNode</li>
  Базовый узел, из которого наследуются все последующие.
  <ul>
    Параметры:
    <li>_type_node - тип узла;</li>
    <li>_type - тип данных;</li>
    <li>_name - идентификатор.</li>
  </ul>
  </p>
  <p><li>Узел FunctionDeclNode : AstNode</li>
  Узел, который представляет функцию.
   <ul>
     Параметры:
    <li>_type_node - тип узла;</li>
    <li>_type - тип данных;</li>
    <li>_name - идентификатор.</li>
    <li>parameters - список параметров функции. Тип данных параметра:  List&ltVarDeclNode&gt </li>
    <li>body - тело функции(арифмитическое выражение). Тип данных параметра:  BinaryOpNode </li>
</p>
   </ul>
       <p>
  <li>Узел VarDeclNode: AstNode</li>
  Узел, представляющий объявление переменной (Тип - Имя переменной).
  <ul>
    Параметры:
    <li>_type_node - тип узла;</li>
    <li>_type - тип данных;</li>
    <li>_name - идентификатор.</li>
  </ul>
  </p>
  <p>
  <li>Узел BinaryOpNode: AstNode</li>
  Узел, представляющий объявление переменной (Тип - Имя переменной).
  <ul>
    Параметры:
    <li>_type_node - тип узла;</li>
    <li>_type - тип данных;</li>
    <li>_name - идентификатор.</li>
    <li>_left_leaf - левое поддерево узла. Тип данных параметра:  List&ltBinaryOpNode&gt</li>
     <li>_right_leaf - правое поддерево узла. Тип данных параметра:  List&ltBinaryOpNode&gt</li>
  </ul>
  </p>
    <li><p>AST:</p></li>
<img align="center" width="1165" height="336" alt="image" src="https://github.com/user-attachments/assets/56824d13-f5f4-4e37-9628-9bf53cb68b89" />
    <li><p>Вывод Ast в прорамме:</p></li>
  <img align="center" width="826" height="738" alt="image" src="https://github.com/user-attachments/assets/2bc204ec-44d5-4974-8435-6c330237caac" />
</ul> 
<h3 align="center"> Тестовые примеры </h3>

<p><img align="center" width="1114" height="894" alt="image" src="https://github.com/user-attachments/assets/72aa1a27-8249-4840-b5e8-912107d08703" /></p>

<img align="center" width="1903" height="314" alt="image" src="https://github.com/user-attachments/assets/727d1099-b7b9-455c-88bc-9e7b28c5ebaa" />






















