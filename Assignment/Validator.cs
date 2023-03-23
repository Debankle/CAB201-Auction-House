using System.Text.RegularExpressions;
namespace Assignment
{
    public class Validator
    {

        public bool ComparePrices(string oldPrice, string newPrice) {
            if (decimal.Parse(oldPrice, System.Globalization.NumberStyles.Currency) < decimal.Parse(newPrice, System.Globalization.NumberStyles.Currency)) {
                return true;
            } else {
                return false;
            }
        }

        public bool ValidateCurrency(string price) {
            if (price[0] != '$') {
                return false;
            }
            var pattern = @"^((([1-9]\d{0,2}(,\d{3})*|[1-9]\d*)(\.\d{2,2})?)|[1-9]\d*|(0\.\d{2,2})|0)$";
            var regex = new Regex(pattern);
            var isValidCurrency = regex.IsMatch(price.Substring(1));
            return isValidCurrency;
        }

        public bool ValidateDescription(string name, string description) {
            if (description == "" || description == name) {
                return false;
            } else {
                return true;
            }
        }

        public bool ValidateProductName(string name) {
            if (name == "") {
                return false;
            } else {
                return true;
            }
        }

        public bool ValidateUnitNo(string unitNo) {
            try {
                int check = int.Parse(unitNo);
                if (0 <= check) {
                    return true;
                } else {
                    return false;
                }
            } catch (FormatException) {
                return false;
            }
        }

        public bool ValidateStreetNo(string streetNo) {
            try {
                int check = int.Parse(streetNo);
                if (check > 0) {
                    return true;
                } else {
                    return false;
                }
            } catch (FormatException) {
                return false;
            }
        }

        public bool ValidateStreetName(string streetName) {
            if (streetName == "") {
                return false;
            } else {
                return true;
            }
        }

        public bool ValidateStreetSuffix(string suffix) {
            if (suffix == "") {
                return false;
            } else {
                return true;
            }
        }

        public bool ValidateCityName(string city) {
            if (city == "") {
                return false;
            } else {
                return true;
            }
        }

        public bool ValidateState(string state) {
            if (state.ToLower() == "qld") {
                return true;
            } else if (state.ToLower() == "nsw") {
                return true;
            } else if (state.ToLower() == "vic") {
                return true;
            } else if (state.ToLower() == "tas") {
                return true;
            } else if (state.ToLower() == "sa") {
                return true;
            } else if (state.ToLower() == "wa") {
                return true;
            } else if (state.ToLower() == "nt") {
                return true;
            } else if (state.ToLower() == "act") {
                return true;
            } else {
                return false;
            }
        }

        public bool ValidatePostCode(string postcode) {
            try {
                int pc = int.Parse(postcode);
                if (pc < 1000 || pc > 9999) {
                    return false;
                } else {
                    return true;
                }
            } catch (FormatException) {
                return false;
            }
        }

        public bool ValidateName(string name) {
            if (name == "") {
                return false;
            } else {
                return true;
            }
        }

        public bool ValidateEmail(string email) {
            string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-.";
            if (email == "") {
                return false;
            } else if (email[0] == '@' || email.EndsWith('@')) {
                return false;
            } else if (email.Count(f => f == '@') != 1) {

                return false;
            }

            string prefix = email.Split('@')[0];
            string suffix = email.Split('@')[1];

            foreach (char c in prefix) {
                if (!validChars.Contains(c) && c != '_') {
                    return false;
                }
            }

            if (prefix.EndsWith('_') || prefix.EndsWith('-') || prefix.EndsWith('.')) {
                return false;
            }

            foreach (char c in suffix) {
                if (!validChars.Contains(c)) {
                    return false;
                }
            }

            if (suffix.Count(f => f == '.') < 1) {
                return false;
            }

            if (suffix[0] == '.' || suffix.EndsWith('.')) {
                return false;
            }

            string[] suffixParts = suffix.Split('.');

            if (!suffixParts[suffixParts.Length-1].All(char.IsLetter)) {
                return false;
            }

            return true;
        }

        public bool ValidatePassword(string password) {
            if (password.Length < 8) { // Is the password longer than 8 chars
                return false;
            } else if (password.Contains(' ')) { // Does the password have a whitespace
                return false;
            } else if (!password.Any(char.IsUpper)) { // Is there an upper-case letter
                return false;
            } else if (!password.Any(char.IsLower)) { // Is there a lower-case letter
                return false;
            } else if (!password.Any(char.IsDigit)) { // Is there a digit
                return false;
            } else if (!password.Any(f => !Char.IsLetterOrDigit(f))) { // Is there a special character
                return false;
            }


            return true;
        }


        public Validator()
        {
        }
    }
}

