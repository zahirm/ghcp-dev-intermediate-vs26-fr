// Module d'inventaire hérité - style volontairement daté, sans tests, sans documentation.
// Cible de démo pour : expliquer, refactoriser, documenter, moderniser, générer des tests, trouver des cas limites.
// Les défauts ci-dessous sont intentionnels.

using Microsoft.Data.Sqlite;

namespace InventoryLegacy;

public class InventoryLegacy
{
    public static string DB = "inventory.db";

    public int get_stock(SqliteConnection conn, string sku)
    {
        var c = conn.CreateCommand();
        c.CommandText = "SELECT qty FROM stock WHERE sku = '" + sku + "'";
        var r = c.ExecuteScalar();
        if (r == null)
            return 0;
        return Convert.ToInt32(r);
    }

    public int adjust(SqliteConnection conn, string sku, int delta)
    {
        var q = get_stock(conn, sku);
        var n = q + delta;
        if (n < 0)
            n = 0;
        var c = conn.CreateCommand();
        c.CommandText = string.Format("UPDATE stock SET qty = {0} WHERE sku = '{1}'", n, sku);
        c.ExecuteNonQuery();
        return n;
    }

    public List<string> reorder_report(SqliteConnection conn, int threshold)
    {
        var c = conn.CreateCommand();
        c.CommandText = "SELECT sku, qty, reorder_point FROM stock";
        var outList = new List<string>();
        using var reader = c.ExecuteReader();
        while (reader.Read())
        {
            var sku = reader.GetString(0);
            var qty = reader.GetInt32(1);
            var rp = reader.GetInt32(2);
            if (qty < rp && qty < threshold)
                outList.Add(sku + " : " + qty.ToString() + " en dessous de " + rp.ToString());
        }
        return outList;
    }

    public double price_with_tax(double price, double rate)
    {
        return price * (1 + rate);
    }

    public void bulk_import(SqliteConnection conn, object[][] rows)
    {
        foreach (var r in rows)
        {
            var c = conn.CreateCommand();
            c.CommandText = string.Format("INSERT INTO stock VALUES ('{0}', {1}, {2})", r[0], r[1], r[2]);
            c.ExecuteNonQuery();
        }
    }
}
