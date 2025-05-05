using System.Collections.Specialized;
using System.Xml.Linq;
namespace Lesson10
{
    public class Program
    {
        private static List<Store> stores = new List<Store>();

        static void Main()
        {
            if (File.Exists("Storage.csv"))
            {
                ReadFile();

                PrintStoresInfo();
                ShowMainMenu();
            }
            else
            {
                Console.WriteLine("Файл Storage.csv не найден.");
            }
        }

        static void PrintStoresInfo()
        {
            foreach (var store in stores)
            {
                Console.WriteLine($"Магазин: {store.Name}, Размер склада: {store.StorageSize} Яблок всего {store.AppleAmount}, Апельсинов всего {store.OrangeAmount}, Яблок на складе: {store.AppleStock}, Продано яблок: {store.AppleSold}, Апельсинов на складе: {store.OrangeStock}, Продано апельсинов: {store.OrangeSold}");
            }
        }

        static void ReadFile()
        {
            stores.Clear();
            StreamReader streamReader = File.OpenText("Storage.csv");

            streamReader.ReadLine();

            while (!streamReader.EndOfStream)
            {
                var data = streamReader.ReadLine().Split(',');

                stores.Add(
                    new Store(
                            name: data[0],
                            storageSize: int.Parse(data[1]),
                            appleAmount: int.Parse(data[2]),
                            orangeAmount: int.Parse(data[3]),
                            appleStock: int.Parse(data[4]),
                            appleSold: int.Parse(data[5]),
                            orangeStock: int.Parse(data[6]),
                            orangeSold: int.Parse(data[7])
                        )
                    );
            }
            streamReader.Close();
        }
        static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nГлавное меню:");
                Console.WriteLine("1. Добавить товар в магазин");
                Console.WriteLine("2. Выйти");
                Console.WriteLine("3. Добавить к продажам");
                Console.WriteLine("4. Вывести csv файл");
                Console.Write("Выберите действие: ");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        HandleAddFruitToStorage();
                        break;
                    case "2":
                        return;
                    case "3":
                        HandleAddSell();
                        break;
                    case "4":
                        ReadFile();
                        PrintStoresInfo();
                        break;

                    default:
                        Console.WriteLine("Неверный ввод, попробуйте еще раз.");
                        break;
                }
            }
        }

        static void HandleAddFruitToStorage()
        {
            Store selectedStore = SelectStore();
            if (selectedStore == null) return;

            int fruitType = SelectFruitType();
            if (fruitType == -1) return;

            Console.Write("\nСколько хотите добавить? ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
            {
                Console.WriteLine("Ошибка! Введите положительное число");
                return;
            }

            bool success = TryAddFruits(selectedStore, fruitType == 1, amount);
            if (success)
            {
                Console.WriteLine($"Успешно добавлено {amount} {(fruitType == 1 ? "яблок" : "апельсинов")} в {selectedStore.Name}");
                UpdateCsvFile();
            }
        }

        static void HandleAddSell()
        {
            Store selectedStore = SelectStore();
            if (selectedStore == null) return;

            int fruitType = SelectFruitType();
            if (fruitType == -1) return;

            Console.Write("\nСколько хотите добавить? ");
            if (!int.TryParse(Console.ReadLine(), out int amount) || amount <= 0)
            {
                Console.WriteLine("Ошибка! Введите положительное число");
                return;
            }

            bool success = TryAddFruitsSell(selectedStore, fruitType == 1, amount);
            if (success)
            {
                Console.WriteLine($"Успешно добавлено {amount} проданных {(fruitType == 1 ? "яблок" : "апельсинов")} в {selectedStore.Name}");
                UpdateCsvFile();
            }
        }

        static bool TryAddFruits(Store store, bool isApple, int amount)
        {
            if (isApple)
            {
                int newTotal = store.AppleStock + amount + store.OrangeStock;
                if (newTotal > store.StorageSize)
                {
                    Console.WriteLine($"Недостаточно места! Можно добавить только {store.StorageSize - (store.AppleStock + store.OrangeStock)}");
                    return false;
                }
                store.AppleStock += amount;
                store.AppleAmount -= amount;
            }
            else
            {
                int newTotal = store.OrangeStock + amount + store.AppleStock;
                if (newTotal > store.StorageSize)
                {
                    Console.WriteLine($"Недостаточно места! Можно добавить только {store.StorageSize - (store.AppleStock + store.OrangeStock)}");
                    return false;
                }
                store.OrangeStock += amount;
                store.OrangeAmount -= amount;
            }
            return true;
        }

        static bool TryAddFruitsSell(Store store, bool isApple, int amount)
        {
            if (isApple)
            {
                if (store.AppleStock < amount) 
                {
                    Console.WriteLine("Ошибка! Нельзя продать больше чем на складе");
                    return false;
                }
                store.AppleSold += amount;
                store.AppleStock -= amount;
            }
            else
            {
                if (store.OrangeStock < amount)
                {
                    Console.WriteLine("Ошибка! Нельзя продать больше чем на складе");
                    return false;
                }
                store.OrangeSold += amount;
                store.OrangeStock -= amount;

            }
            return true;
        }

        static void UpdateCsvFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("Storage.csv"))
                {
                    writer.WriteLine("Название,Размер склада,Яблоки,Апельсины,Яблоки - Склад,Яблоки - Продано,Апельсины - Склад,Апельсины - Продано");

                    foreach (var store in stores)
                    {
                        writer.WriteLine($"{store.Name},{store.StorageSize},{store.AppleAmount},{store.OrangeAmount},{store.AppleStock},{store.AppleSold},{store.OrangeStock},{store.OrangeSold}");
                    }
                }

                Console.WriteLine("Файл успешно обновлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обновлении файла: {ex.Message}");
            }
        }

        static Store SelectStore()
        {
            Console.WriteLine("\nВыберите магазин:");
            for (int i = 0; i < stores.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {stores[i].Name}");
            }

            Console.Write("\nВведите номер магазина: ");
            string userInput = Console.ReadLine();

            if (!int.TryParse(userInput, out int storeIndex))
            {
                Console.WriteLine("Ошибка! Нужно ввести число.");
                return null;
            }

            if (storeIndex < 1 || storeIndex > stores.Count)
            {
                Console.WriteLine($"Ошибка! Номер магазина должен быть от 1 до {stores.Count}.");
                return null;
            }

            var selectedStore = stores[storeIndex - 1];
            Console.WriteLine($"Вы выбрали: {selectedStore.Name}");
            return selectedStore;
        }

        static int SelectFruitType()
        {
            Console.WriteLine("\nКакие фрукты добавляем?");
            Console.WriteLine("1 - Яблоки");
            Console.WriteLine("2 - Апельсины");
            Console.Write("Ваш выбор (введите 1 или 2): ");

            string input = Console.ReadLine();


            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Ошибка! Нужно ввести цифру.");
                return -1;
            }

            if (choice != 1 && choice != 2)
            {
                Console.WriteLine("Ошибка! Введите только 1 или 2.");
                return -1;
            }

            Console.WriteLine(choice == 1 ? "Вы выбрали яблоки" : "Вы выбрали апельсины");
            return choice;
        }
    }
}