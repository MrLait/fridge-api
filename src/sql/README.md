# Запросы SQL

## Проверка для: Перед выполнением этого задания заполни базу так, чтобы в таблицах-словарях было по 5 записей, а в остальных не меньше 10-ти

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge ` -W -s " | " -i ".\src\sql\00_seed_check.sql"
```

## Сделать выборку продуктов по холодильникам, модель которых начинается на А

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge ` -W -s " | " -i ".\src\sql\01_products_in_A_models.sql"
```

## Сделать выборку холодильников, в которых есть продукты в количестве, меньшем чем количество по умолчанию

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge ` -W -s " | " -i ".\src\sql\02_fridges_below_default.sql"
```

## В каком году выпустили холодильник с наибольшей вместимостью (сумма всех продуктов больше всего)

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge ` -W -s " | " -i ".\src\sql\03_max_capacity_model_year.sql"
```

## Выбрать все продукты и имя владельца из холодильника, в котором больше всего наименований продуктов. Именно наименований, не количества

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge ` -W -s " | " -i ".\src\sql\04_owner_and_products_most_names.sql"
```

## Вывести все продукты для холодильника в id 2 (или выбрать определенный Guid)

```powershell
sqlcmd -S localhost,1433 -U sa -P "Fr1dge!p@ssw0rd" -C -d Fridge ` -W -s " | " -i ".\src\sql\05_products_by_fridge.sql"
```

## Вывести все продукты для всех холодильников

```powershell
06_products_all_fridges.sql
```

## Вывести список холодильников и сумму всех продуктов для каждого из них

```powershell
07_fridges_sum_quantity.sql
```

## Вывести имя холодильника, название и год модели, а также кол-во продуктов для каждого из них

```powershell
08_fridge_model_and_counts.sql
```

## Вывести список холодильников, где содержаться продукты, количество которых больше, чем кол-во по умолчанию

```powershell
09_fridges_over_default.sql
```

## Вывести список холодильников и для каждого холодильника кол-во наименований продуктов, количество которых больше, чем кол-во по умолчанию

```powershell
10_fridges_count_names_over_default.sql
```

## Запустить все SQL queries

```powershell
run_all.sql
```

SELECT 'fridge_model' AS [table], COUNT(*) AS cnt FROM fridge_model
UNION ALL SELECT 'products', COUNT(*) FROM products
UNION ALL SELECT 'fridge', COUNT(*) FROM fridge
UNION ALL SELECT 'fridge_products', COUNT(*) FROM fridge_products;