# Aplikacja Konsolowa do Procesowania Zamówień

## Opis Projektu

Aplikacja konsolowa umożliwiająca procesowanie zamówień z obsługą różnych statusów oraz logiką biznesową. Dodatkowo aplikacja posiada testy jednostkowe.

## Funkcjonalności

- Utworzenie przykładowego zamówienia
- Przekazanie zamówienia do magazynu
- Przekazanie zamówienia do wysyłki
- Przegląd zamówień
- Historia procesowania zamówienia
- Wyjście z aplikacji

## Statusy zamówienia

1. Nowe
2. W magazynie
3. W wysyłce
4. Zwrócono do klienta
5. Błąd
6. Zamknięte

## Właściwości zamówienia

- Kwota zamówienia
- Nazwa produktu
- Typ klienta (firma, osoba fizyczna)
- Adres dostawy
- Sposób płatności (karta, gotówka przy odbiorze)

## Warunki biznesowe

1. Zamówienia za nie mniej niż 2500 PLN z płatnością gotówką przy odbiorze zostają zwrócone do klienta przy próbie przekazania do magazynu.
2. Zamówienia przekazane do wysyłki powinny po maksymalnie 5 sekundach zmienić status na „wysłane”.
3. Zamówienia bez adresu dostawy kończą się błędem.
4. Zamówienia muszą najpierw zostać przekazane do magazynu, aby mogły zostać wysłane. (DODANE)

## Dodatkowe rozszerzenia

- Historia procesowania zamówienia
- Własne wyjątki dla specyficznych błędów
- Testy jednostkowe
