﻿using Core.Elements;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Phase3
{
    /// <summary>
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {

        #region Properties

        private List<User> _users = new List<User>();

        private readonly UsersModel _usersModel = new UsersModel();

        #endregion

        #region Constructors

        public AddNewUser(List<User> users)
        {
            InitializeComponent();

            _users = users;

            TBId.Text = _usersModel.GetNextId(users).ToString();
        }

        #endregion

        #region Events

        private void BtnAddUser_Click(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(TBId.Text, out id)) {
                User newUser = new User(id, TBFirstname.Text, TBLastname.Text, TBEmail.Text, PBPassword.Password);
                if (newUser.IsSavable()) {
                    if (_usersModel.Exists("Id", id)) {
                        MessageBox.Show("The id is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else if (_usersModel.Exists("Email", TBEmail.Text)) {
                        MessageBox.Show("This email is already taken.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                    } else {
                        try {
                            _usersModel.AddUser(newUser);
                            _users.Add(newUser);
                            MessageBox.Show("The user has been added.", "User added !", MessageBoxButton.OK, MessageBoxImage.Information);
                            Close();
                        } catch (Exception ex) {
                            MessageBox.Show(ex.Message, "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                } else {
                    SortedDictionary<string, string> errors = newUser.GetInvalidFields();
                    MessageBox.Show(errors.Values.First(), "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            } else {
                MessageBox.Show("The id must be a positive integer.", "Attention !", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

    }

}