INSERT INTO "Entities03" ("Guid0","Guid1","Int0")
SELECT DISTINCT "Guid0","Guid1","Int0" FROM "Entities01";

UPDATE "Entities01" AS e1
SET "Entity03Id" = subquery."Id"
FROM (
    SELECT e3."Id", e3."Guid0", e3."Guid1", e3."Int0"
    FROM "Entities03" e3
) AS subquery
WHERE e1."Guid0" = subquery."Guid0"
  AND e1."Guid1" = subquery."Guid1"
  AND e1."Int0" = subquery."Int0";