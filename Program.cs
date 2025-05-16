using System;
using System.Collections.Generic;

public interface PeminjamanBuku
{
    void PinjamBuku();
    void KembalikanBuku();
}

public abstract class Buku : PeminjamanBuku
{
    public string JudulBuku { get; set; }
    public string Penulis { get; set; }
    public int TahunTerbit { get; set; }
    public bool StatusPinjam { get; protected set; }

    protected Buku(string judul, string pencipta, int tahun)
    {
        JudulBuku = judul;
        Penulis = pencipta;
        TahunTerbit = tahun;
        StatusPinjam = false;
    }

    public abstract void TampilkanDetail();

    public void PinjamBuku()
    {
        StatusPinjam = true;
    }

    public void KembalikanBuku()
    {
        StatusPinjam = false;
    }
}

public class BukuFiksi : Buku
{
    public string Genre { get; set; }

    public BukuFiksi(string judul, string penulis, int tahun, string genre)
        : base(judul, penulis, tahun)
    {
        Genre = genre;
    }

    public override void TampilkanDetail()
    {
        Console.WriteLine($"Novel {Genre}: {JudulBuku}\n" +
                          $"Penulis: {Penulis}, Tahun: {TahunTerbit}\n" +
                          $"Status: {(StatusPinjam ? "Dipinjam" : "Tersedia")}\n");
    }
}

public class BukuNonFiksi : Buku
{
    public string Bidang { get; set; }

    public BukuNonFiksi(string judul, string penulis, int tahun, string bidang)
        : base(judul, penulis, tahun)
    {
        Bidang = bidang;
    }

    public override void TampilkanDetail()
    {
        Console.WriteLine($"Buku {Bidang}: {JudulBuku}\n" +
                          $"Penulis: {Penulis}, Tahun: {TahunTerbit}\n" +
                          $"Status: {(StatusPinjam ? "Dipinjam" : "Tersedia")}\n");
    }
}

public class Majalah : Buku
{
    public int Edisi { get; set; }

    public Majalah (string judul, string penerbit, int tahun, int edisi)
        : base(judul, penerbit, tahun)
    {
        Edisi = edisi;
    }

    public override void TampilkanDetail()
    {
        Console.WriteLine($"Terbitan #{Edisi}: {JudulBuku}\n" +
                          $"Penerbit: {Penulis}, Tahun: {TahunTerbit}\n" +
                          $"Status: {(StatusPinjam ? "Dipinjam" : "Tersedia")}\n");
    }
}

public class Perpustakaan
{
    private List<Buku> inventaris = new List<Buku>();
    private List<Buku> dalamPinjaman = new List<Buku>();

    public void TambahKeInventaris(Buku item)
    {
        inventaris.Add(item);
        Console.WriteLine($"\n'{item.JudulBuku}' berhasil ditambahkan ke rak buku");
    }

    public void PerbaruiStatus(string pencarian, string JudulBaru, string PenulisBaru, int TahunTerbitTerbaru)
    {
        var target = inventaris.Find(i => i.JudulBuku.Equals(pencarian, StringComparison.OrdinalIgnoreCase));
        if (target != null)
        {
            target.JudulBuku = JudulBaru;
            target.Penulis = PenulisBaru;
            target.TahunTerbit = TahunTerbitTerbaru;
            Console.WriteLine($"\n Data '{pencarian}' sudah diperbarui");
        }
        else
        {
            Console.WriteLine("\n maaf bang buku tidak ditemukan");
        }
    }

    public void TampilkanInventaris()
    {
        Console.WriteLine("\n=== DAFTAR KOLEKSI PERPUSTAKAAN ===");
        foreach (var item in inventaris)
        {
            item.TampilkanDetail();
        }
    }

    public void ProsesPinjam(string judul)
    {
        if (dalamPinjaman.Count >= 3)
        {
            Console.WriteLine("\nmaksimal boleh meminjam 3 kak");
            return;
        }

        var item = inventaris.Find(i => i.JudulBuku.Equals(judul, StringComparison.OrdinalIgnoreCase));

        if (item == null)
        {
            Console.WriteLine("\nBuku Tidak Tersedia");
            return;
        }

        if (item.StatusPinjam)
        {
            Console.WriteLine("\n Buku Lagi dipinjam");
            return;
        }

        item.PinjamBuku();
        dalamPinjaman.Add(item);
        Console.WriteLine($"\n Buku '{judul}' Berhasil Dipinjam");
    }

    public void ProsesPengembalian(string judul)
    {
        var item = dalamPinjaman.Find(i => i.JudulBuku.Equals(judul, StringComparison.OrdinalIgnoreCase));
        if (item != null)
        {
            item.KembalikanBuku();
            dalamPinjaman.Remove(item);
            Console.WriteLine($"\n Buku '{judul}' sudah dikembalikan");
        }
        else
        {
            Console.WriteLine("\nBuku Tidak Ada Di Daftar Pinjaman");
        }
    }

    public void TampilkanPinjaman()
    {
        Console.WriteLine("\n=== DAFTAR BUKU PINJAMAN ===");
        foreach (var item in dalamPinjaman)
        {
            item.TampilkanDetail();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Perpustakaan perpustakaan = new Perpustakaan();
        bool aktif = true;

        while (aktif)
        {
            Console.WriteLine("\n===== SISTEM MANAJEMEN PERPUSTAKAAN =====");
            Console.WriteLine("1. Tampilkan Semua Buku");
            Console.WriteLine("2. Tambah Koleksi Buku Baru");
            Console.WriteLine("3. Pinjam Buku");
            Console.WriteLine("4. Kembalikan Buku");
            Console.WriteLine("5. Lihat Buku Yang Sedang Dipinjam");
            Console.WriteLine("6. Keluar");
            Console.Write("Pilih menu: ");

            switch (Console.ReadLine())
            {
                case "1":
                    perpustakaan.TampilkanInventaris();
                    break;

                case "2":
                    TambahKoleksiMenu(perpustakaan);
                    break;

                case "3":
                    ProsesPinjamMenu(perpustakaan);
                    break;

                case "4":
                    ProsesKembaliMenu(perpustakaan);
                    break;

                case "5":
                    perpustakaan.TampilkanPinjaman();
                    break;

                case "6":
                    aktif = false;
                    Console.WriteLine("\nTerimakasih Ya Kembali Lagi Kapan Kapan");
                    break;

                default:
                    Console.WriteLine("\nSalah Input Min");
                    break;
            }
        }
    }

    static void TambahKoleksiMenu(Perpustakaan sys)
    {
        Console.WriteLine("\nJenis Buku: ");
        Console.WriteLine("1. Buku Fiksi ");
        Console.WriteLine("2. Buku Non Fiksi ");
        Console.WriteLine("3. Majalah ");
        Console.Write("Pilih jenis: ");

        var jenis = Console.ReadLine();
        Console.Write("Judul Buku : ");
        var judul = Console.ReadLine();
        Console.Write("Penulis: ");
        var pencipta = Console.ReadLine();
        Console.Write("Tahun Terbit : ");
        int tahun = int.Parse(Console.ReadLine());

        switch (jenis)
        {
            case "1":
                Console.Write("Genre : ");
                sys.TambahKeInventaris(new BukuFiksi(judul, pencipta, tahun, Console.ReadLine()));
                break;

            case "2":
                Console.Write("Bidang : ");
                sys.TambahKeInventaris(new BukuNonFiksi(judul, pencipta, tahun, Console.ReadLine()));
                break;

            case "3":
                Console.Write("Edisi: ");
                sys.TambahKeInventaris(new Majalah(judul, pencipta, tahun, int.Parse(Console.ReadLine())));
                break;

            default:
                Console.WriteLine("\nJenis Buku Yang Dipilih Tidak Ada");
                break;
        }
    }

    static void ProsesPinjamMenu(Perpustakaan sys)
    {
        Console.Write("\nMasukkan judul buku yang ingin dipinjam: ");
        sys.ProsesPinjam(Console.ReadLine());
    }

    static void ProsesKembaliMenu(Perpustakaan sys)
    {
        Console.Write("\nMasukkan judul buku yang akan dikembalikan: ");
        sys.ProsesPengembalian(Console.ReadLine());
    }
}