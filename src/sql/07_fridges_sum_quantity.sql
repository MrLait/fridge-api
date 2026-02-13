SELECT
    f.name              as fridge_name,
    SUM(fp.quantity)    as quantity_sum
FROM fridge f
JOIN fridge_products fp ON fp.fridge_id = f.id
GROUP BY f.id, f.name
ORDER BY quantity_sum DESC;