SELECT TOP 1 WITH TIES
    f.id as fridge_id,
    fm.name as fridge_model,
    SUM(fp.quantity) as product_quantity,
    fm.year as fridge_year
FROM fridge f
JOIN fridge_model fm    ON f.model_id = fm.id
JOIN fridge_products fp ON f.id = fp.fridge_id
GROUP BY f.id, fm.name, fm.year
ORDER BY SUM(fp.quantity) DESC;