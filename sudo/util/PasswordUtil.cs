using System;
using System.Security;
using System.Text;

namespace sudo.util
{
    public static class PasswordUtil
    {
        public static SecureString Get(string domain, string username) {
            SecureString encryptedPassword = new SecureString();
            StringBuilder passwordInClearText = Flags.Has(Flags.DEBUG) ? new StringBuilder() : null;

            Console.Write($"Please enter the password for {domain}\\{username}: ");

            int i = 0;
            bool insertMode = false;
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(-1); // exit, do nothing
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Enter)
                    break; // read complete

                if (keyInfo.Key == ConsoleKey.Clear)
                {
                    encryptedPassword.Clear();
                    passwordInClearText?.Clear();
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (i > 0)
                    {
                        int removeAt = i;
                        encryptedPassword.RemoveAt(removeAt);
                        passwordInClearText?.Remove(removeAt - 1, 1);
                        i--;
                    }

                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Delete)
                {
                    int removeAt = i + 1;
                    if (removeAt < encryptedPassword.Length)
                    {
                        encryptedPassword.RemoveAt(removeAt);
                        passwordInClearText?.Remove(removeAt, 1);
                    }
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Insert)
                {
                    insertMode = !insertMode;
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.Home)
                {
                    i = 0;
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.End)
                {
                    i = encryptedPassword.Length;
                    continue;
                }

                if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (i > 0)
                        i--;

                    continue;
                }

                if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (i < encryptedPassword.Length)
                        i++;

                    continue;
                }

                if (keyInfo.KeyChar != '\0')
                {
                    if (i == encryptedPassword.Length)
                    {
                        encryptedPassword.AppendChar(keyInfo.KeyChar);
                        passwordInClearText?.Append(keyInfo.KeyChar);
                        i++;
                    }
                    else
                    {
                        if (insertMode)
                        {
                            encryptedPassword.SetAt(i, keyInfo.KeyChar);
                            if (passwordInClearText != null)
                                passwordInClearText[i] = keyInfo.KeyChar;
                        }
                        else
                        {
                            encryptedPassword.InsertAt(i, keyInfo.KeyChar);
                            if (passwordInClearText != null) {
                                for (int x = passwordInClearText.Length; x > i; x--) {
                                    if (x == passwordInClearText.Length)
                                    {
                                        passwordInClearText.Append(passwordInClearText[x - 1]);
                                    }
                                    else
                                    {
                                        passwordInClearText[x] = passwordInClearText[x - 1];
                                    }
                                }
                                passwordInClearText[i] = keyInfo.KeyChar;
                            }
                        }
                        i++;
                    }
                }
            }

            return encryptedPassword;
        }
    }
}
