SELECT
    f.name          as fridge_name,
    p.name          as product_name,
    fp.quantity     as product_quantity
FROM fridge_products fp
JOIN products p     ON fp.product_id = p.id
JOIN fridge f      ON fp.fridge_id = f.id
ORDER BY f.name, p.name;