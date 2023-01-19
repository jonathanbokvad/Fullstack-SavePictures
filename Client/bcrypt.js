'use strict';
const bcrypt = require('bcrypt');

exports.encrypt = (password) => {
    const saltRounds = 10;
    return bcrypt.hashSync(password, saltRounds);
};