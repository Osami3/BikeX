-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Хост: db
-- Время создания: Дек 01 2022 г., 05:34
-- Версия сервера: 10.9.3-MariaDB-1:10.9.3+maria~ubu2204
-- Версия PHP: 8.0.19

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `19099_BikeX`
--

-- --------------------------------------------------------

--
-- Структура таблицы `active_zakaz`
--

CREATE TABLE `active_zakaz` (
  `id_active_zakaz` int(11) NOT NULL,
  `id_client` int(11) NOT NULL,
  `id_bike` int(11) NOT NULL,
  `id_sotrudnik` int(11) NOT NULL,
  `date_start` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Структура таблицы `bike`
--

CREATE TABLE `bike` (
  `id_bike` int(11) NOT NULL,
  `number_bike` varchar(10) NOT NULL,
  `status` int(1) DEFAULT NULL,
  `id_size` int(11) NOT NULL,
  `price_min` int(11) NOT NULL,
  `name_bike` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `bike`
--

INSERT INTO `bike` (`id_bike`, `number_bike`, `status`, `id_size`, `price_min`, `name_bike`) VALUES
(3, '0gwrMj4nMx', 1, 3, 7, 'Дорожный велосипед'),
(4, 'QtwcTn4rck', 1, 4, 40, 'Горный велосипед красный'),
(5, 'MZi0LP5P3H', 1, 5, 8, 'Велосипед XL синий'),
(6, 'cvxuYf9DQX', 1, 4, 8, 'Велосипед L красный'),
(7, '9ROLYZHsPn', 1, 2, 4, 'Велосипед средний'),
(8, 'Русь', 1, 1, 1, 'jhfdgfgkdgdhf'),
(9, 'hklhklhkli', 0, 1, 6677, 'uuui'),
(10, 'gui  ', 0, 1, 87, '  ');

-- --------------------------------------------------------

--
-- Структура таблицы `client`
--

CREATE TABLE `client` (
  `id_client` int(5) NOT NULL,
  `name` varchar(26) NOT NULL,
  `phone` varchar(11) NOT NULL,
  `email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `client`
--

INSERT INTO `client` (`id_client`, `name`, `phone`, `email`) VALUES
(3, 'Андрей', '89526164256', 'graukoddecexei-6887@yopmail.com'),
(4, 'Абдул', '89986240143', 'grozoifrouyuya-9811@yopmail.com'),
(5, 'Артур', '89902743839', 'canicapanni-6956@yopmail.com'),
(6, 'Евген', '89936831478', 'hellutraubrauheu-6625@yopmail.com'),
(7, ' ', '89989467011', 'zewufavajo-6685@yopmail.com'),
(8, 'Максим', '89909286842', 'haududutteuro-8208@yopmail.com'),
(9, 'Анна', '89912626199', 'jeuppetriwinnei-2691@yopmail.com'),
(10, 'Оля', '89960071149', 'heicivoifutoi-5832@yopmail.com'),
(11, 'Анна', '89928210229', ' '),
(12, 'Лера', '89995434964', 'priquoiprajauzo-4728@yopmail.com'),
(13, 'Антон', '89900150183', 'wayeinaubiwa-6896@yopmail.com'),
(15, 'dima', '89996262067', 'dima@mail.ru'),
(23, 'Иван', '89966221348', 'zarozillouddou-6172@yopmail.com'),
(28, 'Максим Бездушев', '89071300386', 'maxim_bezdushev@mail.ru'),
(30, 'dsfsd', '33333333333', 'sdf');

-- --------------------------------------------------------

--
-- Структура таблицы `doljnost_sotrudnik`
--

CREATE TABLE `doljnost_sotrudnik` (
  `id_doljnost` int(11) NOT NULL,
  `doljnost` varchar(50) NOT NULL,
  `salary` int(6) NOT NULL,
  `hour` int(3) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `doljnost_sotrudnik`
--

INSERT INTO `doljnost_sotrudnik` (`id_doljnost`, `doljnost`, `salary`, `hour`) VALUES
(1, 'Директор', 65000, 180),
(2, 'Администратор', 25000, 180),
(3, 'Рабочий', 20000, 90);

-- --------------------------------------------------------

--
-- Структура таблицы `passport_sotrudnik`
--

CREATE TABLE `passport_sotrudnik` (
  `id_passport` int(11) NOT NULL,
  `number` int(4) NOT NULL,
  `seria` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `passport_sotrudnik`
--

INSERT INTO `passport_sotrudnik` (`id_passport`, `number`, `seria`) VALUES
(1, 2517, 542745),
(2, 2165, 346643),
(3, 1235, 752532),
(4, 1243, 345321);

-- --------------------------------------------------------

--
-- Структура таблицы `size_bike`
--

CREATE TABLE `size_bike` (
  `id_size` int(11) NOT NULL,
  `size_rama` int(11) NOT NULL,
  `height_sm` int(3) NOT NULL,
  `name_size` varchar(2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `size_bike`
--

INSERT INTO `size_bike` (`id_size`, `size_rama`, `height_sm`, `name_size`) VALUES
(1, 35, 155, 'XS'),
(2, 40, 165, 'S'),
(3, 46, 178, 'M'),
(4, 51, 185, 'L'),
(5, 56, 195, 'XL');

-- --------------------------------------------------------

--
-- Структура таблицы `sotrudnik`
--

CREATE TABLE `sotrudnik` (
  `id_sotrudnik` int(11) NOT NULL,
  `id_doljnost` int(1) NOT NULL,
  `name` varchar(11) NOT NULL,
  `surname` varchar(11) NOT NULL,
  `partronymic` varchar(11) NOT NULL,
  `phone` varchar(11) NOT NULL,
  `id_passport` int(1) NOT NULL,
  `password` int(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `sotrudnik`
--

INSERT INTO `sotrudnik` (`id_sotrudnik`, `id_doljnost`, `name`, `surname`, `partronymic`, `phone`, `id_passport`, `password`) VALUES
(1, 1, 'Марк', 'Быков', 'Ермакович', '89032066499', 1, 562992),
(2, 2, 'Милен', 'Швец', 'Кириллович', '89868109390', 2, 604907),
(3, 2, 'Дарья', 'Островская', 'Дмитриевна', '89285749470', 3, 171298),
(4, 3, 'Богдан', 'Наумов', 'Игоревич', '89370996465', 4, 752813);

-- --------------------------------------------------------

--
-- Структура таблицы `zakaz`
--

CREATE TABLE `zakaz` (
  `id_zakaz` int(11) NOT NULL,
  `id_client_zakaz` int(11) NOT NULL,
  `id_bike_zakaz` int(11) NOT NULL,
  `id_sotrudnik_zakaz` int(2) NOT NULL,
  `min` int(2) NOT NULL,
  `price` int(11) NOT NULL,
  `date_start` datetime NOT NULL,
  `date_end` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп данных таблицы `zakaz`
--

INSERT INTO `zakaz` (`id_zakaz`, `id_client_zakaz`, `id_bike_zakaz`, `id_sotrudnik_zakaz`, `min`, `price`, `date_start`, `date_end`) VALUES
(2, 4, 7, 2, 3, 0, '2022-11-15 15:21:18', '2022-11-15 18:24:16'),
(3, 6, 3, 3, 5, 0, '2022-11-16 15:24:46', '2022-11-16 20:20:46'),
(7, 15, 7, 2, 0, 0, '2022-11-30 12:20:13', '2022-11-30 12:20:34'),
(8, 15, 3, 3, 0, 0, '2022-11-30 12:37:05', '2022-11-30 12:37:43'),
(9, 15, 4, 3, 0, 0, '2022-11-30 13:28:19', '2022-11-30 13:28:39'),
(10, 15, 5, 2, 1, 8, '2022-12-01 09:33:56', '2022-12-01 09:34:33');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `active_zakaz`
--
ALTER TABLE `active_zakaz`
  ADD PRIMARY KEY (`id_active_zakaz`),
  ADD KEY `id_bike` (`id_bike`),
  ADD KEY `id_client` (`id_client`,`id_sotrudnik`),
  ADD KEY `id_client_2` (`id_client`),
  ADD KEY `id_sotrudnik` (`id_sotrudnik`);

--
-- Индексы таблицы `bike`
--
ALTER TABLE `bike`
  ADD PRIMARY KEY (`id_bike`),
  ADD KEY `id_size` (`id_size`);

--
-- Индексы таблицы `client`
--
ALTER TABLE `client`
  ADD PRIMARY KEY (`id_client`),
  ADD UNIQUE KEY `id_client_2` (`id_client`),
  ADD KEY `id_client` (`id_client`);

--
-- Индексы таблицы `doljnost_sotrudnik`
--
ALTER TABLE `doljnost_sotrudnik`
  ADD PRIMARY KEY (`id_doljnost`);

--
-- Индексы таблицы `passport_sotrudnik`
--
ALTER TABLE `passport_sotrudnik`
  ADD PRIMARY KEY (`id_passport`);

--
-- Индексы таблицы `size_bike`
--
ALTER TABLE `size_bike`
  ADD PRIMARY KEY (`id_size`);

--
-- Индексы таблицы `sotrudnik`
--
ALTER TABLE `sotrudnik`
  ADD PRIMARY KEY (`id_sotrudnik`),
  ADD KEY `id_doljnost` (`id_doljnost`),
  ADD KEY `id_passport` (`id_passport`);

--
-- Индексы таблицы `zakaz`
--
ALTER TABLE `zakaz`
  ADD PRIMARY KEY (`id_zakaz`),
  ADD KEY `id_client` (`id_client_zakaz`),
  ADD KEY `id_bike` (`id_bike_zakaz`),
  ADD KEY `id_sotrudnik` (`id_sotrudnik_zakaz`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `active_zakaz`
--
ALTER TABLE `active_zakaz`
  MODIFY `id_active_zakaz` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT для таблицы `bike`
--
ALTER TABLE `bike`
  MODIFY `id_bike` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT для таблицы `client`
--
ALTER TABLE `client`
  MODIFY `id_client` int(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT для таблицы `doljnost_sotrudnik`
--
ALTER TABLE `doljnost_sotrudnik`
  MODIFY `id_doljnost` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT для таблицы `passport_sotrudnik`
--
ALTER TABLE `passport_sotrudnik`
  MODIFY `id_passport` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `size_bike`
--
ALTER TABLE `size_bike`
  MODIFY `id_size` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `sotrudnik`
--
ALTER TABLE `sotrudnik`
  MODIFY `id_sotrudnik` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `zakaz`
--
ALTER TABLE `zakaz`
  MODIFY `id_zakaz` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `active_zakaz`
--
ALTER TABLE `active_zakaz`
  ADD CONSTRAINT `active_zakaz_ibfk_1` FOREIGN KEY (`id_bike`) REFERENCES `bike` (`id_bike`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `active_zakaz_ibfk_2` FOREIGN KEY (`id_client`) REFERENCES `client` (`id_client`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `active_zakaz_ibfk_3` FOREIGN KEY (`id_sotrudnik`) REFERENCES `sotrudnik` (`id_sotrudnik`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `bike`
--
ALTER TABLE `bike`
  ADD CONSTRAINT `bike_ibfk_1` FOREIGN KEY (`id_size`) REFERENCES `size_bike` (`id_size`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `passport_sotrudnik`
--
ALTER TABLE `passport_sotrudnik`
  ADD CONSTRAINT `passport_sotrudnik_ibfk_1` FOREIGN KEY (`id_passport`) REFERENCES `sotrudnik` (`id_passport`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `sotrudnik`
--
ALTER TABLE `sotrudnik`
  ADD CONSTRAINT `sotrudnik_ibfk_1` FOREIGN KEY (`id_doljnost`) REFERENCES `doljnost_sotrudnik` (`id_doljnost`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Ограничения внешнего ключа таблицы `zakaz`
--
ALTER TABLE `zakaz`
  ADD CONSTRAINT `zakaz_ibfk_1` FOREIGN KEY (`id_sotrudnik_zakaz`) REFERENCES `sotrudnik` (`id_sotrudnik`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `zakaz_ibfk_2` FOREIGN KEY (`id_client_zakaz`) REFERENCES `client` (`id_client`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `zakaz_ibfk_3` FOREIGN KEY (`id_bike_zakaz`) REFERENCES `bike` (`id_bike`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
