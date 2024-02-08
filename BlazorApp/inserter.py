import sqlite3
import csv

# Connect to SQLite database
conn = sqlite3.connect('data.sqlite')
cur = conn.cursor()

# Read CSV file
with open('definitions.csv', newline='\r\n', encoding="utf-8") as csvfile:
    reader = csv.reader(csvfile, delimiter=';')
    for row in reader:
        if row[1] == "!!!ÄRA SISESTA ANCsse!!!":
            cur.execute("INSERT INTO AncClassifierMappings (ProductName, AncClassifierId, Multiplier) VALUES (?, ?, ?)", (row[0], -1, row[2]))
            continue
        cur.execute("SELECT Id FROM AncClassifiers WHERE Name = ?", (row[1],))
        result = cur.fetchone()
        if result:
            ref_id = result[0]
            print(row[0])
            try:
                cur.execute("INSERT INTO AncClassifierMappings (ProductName, AncClassifierId, Multiplier) VALUES (?, ?, ?)", (row[0], ref_id, row[2]))
            except(sqlite3.IntegrityError):
                print("Duplicate" + row[0])
        else:
            # Handle case where no matching ID is found
            print(f"No matching ID found for {row[1]}")

# Commit changes and close connection
conn.commit()
conn.close()
