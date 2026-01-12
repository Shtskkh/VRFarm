#set document(
  title: "Дизайн документ игры \"VR Farm\"",
  author: (
    "Шацких Алексей Евгеньевич", 
    "Золотухина Анастасия Максимовна",
    "Муравьёв Никита Максимович",
    "Зимина Юлия Игоревна"
  ),
)

#set page(
  margin: (
    top: 2cm,
    left: 3cm,
    right: 1.5cm,
    bottom: 2cm,
  )
)

#set text(
  font: "Times New Roman",
  size: 14pt,
  lang: "ru",
  hyphenate: false,
)

#set par(
  first-line-indent: (amount: 1.25cm, all: true),
  leading: 1.5em,
  justify: true,
)

#show heading: it => [
  #set align(center)

  #set text(
    size: 14pt,
  )

  #block[
    #it
    #v(1em)
  ]
]

= Дизайн документ игры "VR Farm"