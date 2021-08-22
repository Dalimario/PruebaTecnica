var express = require('express')
  , Chart = require('./chart')

var sendfile = function(filename, root) {
  return function(_, res) {
    res.sendfile(filename, { root: root })
  }
}

var jquery = sendfile('/jquery-1.6.2.min.js', __dirname)
  , peity = sendfile('/jquery.peity.js', __dirname + '/..')
  , style = sendfile('/style.css', __dirname)

var show = function(req, res) {
  var id = req.params.id
    , chart = Chart.find(id)

  if (chart) {
    res.render('chart', {
      options: chart.optionsString(),
      text: chart.text,
      title: 'Chart: ' + id,
      type: chart.type
    })
  } else {
    res
      .status(404)
      .end()
  }
}

var app = express()
  .set('view engine', 'ejs')
  .set('views', __dirname)
  .get('/jquery.min.js', jquery)
  .get('/jquery.peity.js', peity)
  .get('/style.css', style)
  .get('/charts/:id', show)

module.exports = app
