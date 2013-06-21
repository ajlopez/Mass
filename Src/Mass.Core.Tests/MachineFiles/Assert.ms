
var assert
assert = require("assert")

var a
a = 1

assert.equal(1, a)
assert.notEqual(2, a)

assert.true(true)
assert.true(1)
assert.true("Hello world")

assert.false(null)
assert.false(false)

