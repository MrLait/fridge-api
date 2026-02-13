SELECT
    f.name              as fridge_name,
    fm.name             as model_name,
    fm.year             as model_year,
    SUM(fp.quantity)    as product_quantity_sum,
    COUNT(DISTINCT fp.product_id) as product_name_cnt
FROM fridge f
JOIN fridge_model fm    ON fm.id = f.model_id
JOIN fridge_products fp ON fp.fridge_id = f.id
GROUP BY f.id, f.name, fm.name, fm.year
ORDER BY fm.year, product_quantity_sum DESC;